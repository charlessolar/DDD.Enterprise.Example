using Demo.Library.Security;
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

        public static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            Contract.Requires(fields != null);
            Contract.Requires(fields.Count != 0);

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

        // Generates a new type for use with IHow - a security action
        // Spits out a type who calls an Auth check for everymethod call and property read/write
        public static Type GetDynamicSecuredType<T>(Dictionary<string, Type> fields) where T : IWhat
        {
            Contract.Requires(fields != null);
            Contract.Requires(fields.Count != 0);

            try
            {
                Monitor.Enter(builtTypes);
                string typeKey = GetTypeKey(fields);

                if (builtTypes.ContainsKey(typeKey))
                    return builtTypes[typeKey].Item2;

                var whatType = typeof(T);

                string typeName = "DynamicLinqType" + builtTypes.Count.ToString();
                TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable, whatType, Type.EmptyTypes);

                foreach (var field in fields)
                {
                    var fieldField = typeBuilder.DefineField("_" + field.Key, field.Value, FieldAttributes.Private);
                    var fieldProperty = typeBuilder.DefineProperty(field.Key, PropertyAttributes.HasDefault, field.Value, null);

                    var authorizeMethod = whatType.GetMethod("Authorize", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

                    var getter = typeBuilder.DefineMethod("get_" + field.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, field.Value, Type.EmptyTypes);
                    var getterIL = getter.GetILGenerator();

                    getterIL.Emit(OpCodes.Ldarg_0);
                    getterIL.Emit(OpCodes.Callvirt, authorizeMethod);

                    getterIL.Emit(OpCodes.Ldarg_0);
                    getterIL.Emit(OpCodes.Ldfld, fieldField);
                    getterIL.Emit(OpCodes.Ret);

                    var setter = typeBuilder.DefineMethod("set_" + field.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { field.Value });
                    var setterIL = setter.GetILGenerator();

                    setterIL.Emit(OpCodes.Ldarg_0);
                    setterIL.Emit(OpCodes.Callvirt, authorizeMethod);

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