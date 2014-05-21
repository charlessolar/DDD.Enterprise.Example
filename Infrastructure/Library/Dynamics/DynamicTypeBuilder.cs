using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Demo.Library.Dynamics
{
    // https://github.com/thiscode/DynamicSelectExtensions
    public static class DynamicTypeBuilder
    {
        private static AssemblyName assemblyName = new AssemblyName() { Name = "DynamicLinqTypes" };
        private static ModuleBuilder moduleBuilder = null;
        private static Dictionary<string, Tuple<string, Type>> builtTypes = new Dictionary<string, Tuple<string, Type>>();

        static DynamicTypeBuilder()
        {
            moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            string key = string.Empty;
            foreach (var field in fields.OrderBy(v => v.Key).ThenBy(v => v.Value.Name))
                key += field.Key + ";" + field.Value.Name + ";";
            return key;
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields, Type basetype, Type[] interfaces)
        {
            if (null == fields)
                throw new ArgumentNullException("fields");
            if (0 == fields.Count)
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

            try
            {
                Monitor.Enter(builtTypes);
                string typeKey = GetTypeKey(fields);

                if (builtTypes.ContainsKey(typeKey))
                    return builtTypes[typeKey].Item2;

                string typeName = "DynamicLinqType" + builtTypes.Count.ToString();
                TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable, null, Type.EmptyTypes);

                foreach (var field in fields)
                {
                    var fieldField = typeBuilder.DefineField("_" + field.Key, field.Value, FieldAttributes.Private);
                    var fieldProperty = typeBuilder.DefineProperty(field.Key, PropertyAttributes.HasDefault, field.Value, null);

                    var getter = typeBuilder.DefineMethod("get_" + field.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, field.Value, Type.EmptyTypes);
                    var getterIL = getter.GetILGenerator();
                    getterIL.Emit(OpCodes.Ldarg_0);
                    getterIL.Emit(OpCodes.Ldfld, fieldField);
                    getterIL.Emit(OpCodes.Ret);

                    var setter = typeBuilder.DefineMethod("set_" + field.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { field.Value });
                    var setterIL = setter.GetILGenerator();
                    setterIL.Emit(OpCodes.Ldarg_0);
                    setterIL.Emit(OpCodes.Ldarg_1);
                    setterIL.Emit(OpCodes.Stfld, fieldField);
                    setterIL.Emit(OpCodes.Ret);

                    fieldProperty.SetGetMethod(getter);
                    fieldProperty.SetSetMethod(setter);
                }
                builtTypes[typeKey] = new Tuple<string, Type>(typeName, typeBuilder.CreateType());

                return builtTypes[typeKey].Item2;
            }
            catch
            {
                throw;
            }
            finally
            {
                Monitor.Exit(builtTypes);
            }
        }
    }
}