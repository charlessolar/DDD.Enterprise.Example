using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Demo.Library.Dynamics
{
    // https://github.com/thiscode/DynamicSelectExtensions
    public static class DynamicTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new AssemblyName() { Name = "DynamicLinqTypes" };
        private static readonly ModuleBuilder ModuleBuilder = null;
        private static readonly Dictionary<string, Tuple<string, Type>> BuiltTypes = new Dictionary<string, Tuple<string, Type>>();

        static DynamicTypeBuilder()
        {
            ModuleBuilder = Thread.GetDomain().DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(AssemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            string key = string.Empty;
            foreach (var field in fields.OrderBy(v => v.Key).ThenBy(v => v.Value.Name))
                key += field.Key + ";" + field.Value.Name + ";";
            return key;
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            Contract.Requires(fields != null);
            Contract.Requires(fields.Count != 0);

            try
            {
                Monitor.Enter(BuiltTypes);
                string typeKey = GetTypeKey(fields);

                if (BuiltTypes.ContainsKey(typeKey))
                    return BuiltTypes[typeKey].Item2;

                string typeName = "DynamicLinqType" + BuiltTypes.Count.ToString();
                TypeBuilder typeBuilder = ModuleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable, null, Type.EmptyTypes);

                foreach (var field in fields)
                {
                    var fieldField = typeBuilder.DefineField("_" + field.Key, field.Value, FieldAttributes.Private);
                    var fieldProperty = typeBuilder.DefineProperty(field.Key, PropertyAttributes.HasDefault, field.Value, null);

                    var getter = typeBuilder.DefineMethod("get_" + field.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, field.Value, Type.EmptyTypes);
                    var getterIl = getter.GetILGenerator();
                    getterIl.Emit(OpCodes.Ldarg_0);
                    getterIl.Emit(OpCodes.Ldfld, fieldField);
                    getterIl.Emit(OpCodes.Ret);

                    var setter = typeBuilder.DefineMethod("set_" + field.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { field.Value });
                    var setterIl = setter.GetILGenerator();
                    setterIl.Emit(OpCodes.Ldarg_0);
                    setterIl.Emit(OpCodes.Ldarg_1);
                    setterIl.Emit(OpCodes.Stfld, fieldField);
                    setterIl.Emit(OpCodes.Ret);

                    fieldProperty.SetGetMethod(getter);
                    fieldProperty.SetSetMethod(setter);
                }
                BuiltTypes[typeKey] = new Tuple<string, Type>(typeName, typeBuilder.CreateType());

                return BuiltTypes[typeKey].Item2;
            }
            catch
            {
                throw;
            }
            finally
            {
                Monitor.Exit(BuiltTypes);
            }
        }
    }
}