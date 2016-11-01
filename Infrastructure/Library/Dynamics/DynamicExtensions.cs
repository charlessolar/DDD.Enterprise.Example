using Demo.Library.Dynamics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Demo.Library.Extensions
{
    // https://github.com/thiscode/DynamicSelectExtensions
    public static class DynamicExtensions
    {
        /// <summary>
        /// Use this to select only specific fields, instead fetching the whole entity.
        /// </summary>
        /// <example><code>
        /// var query =
        ///     YOUR_QUERYABLE_OBJECT
        ///     .SelectDynamicFields(new List<string>(){
        ///         "Property1",
        ///         "Property2",
        ///     });
        ///
        /// dynamic FirstObj = query.FirstOrDefault();
        /// Console.WriteLine(FirstObj.Property1); //Name of the member will validated in runtime!
        /// </code></example>
        /// <typeparam name="T">Type of Source IQueryable</typeparam>
        /// <param name="source">Source IQueryable</param>
        /// <param name="propertyNames">List of Property-Names you want to Select</param>
        /// <returns>A dynamic IQueryable Object. The object includes all Property-Names you have given as Fields.</returns>
        public static IQueryable<dynamic> SelectPartial<T>(this IQueryable<T> source, IEnumerable<string> propertyNames)
        {
            Contract.Requires(source != null);

            //Here we do something similar to
            //
            //  Select(source => new {
            //      property1 = source.property1,
            //      property2 = source.property2,
            //      [...]
            //  })
            //
            //We build here firstly the Expression needed by the Select-Method dynamicly.
            //Beyond this we build even the class dynamicly. The class includes only
            //the Properties we want to project. The difference is, that the class is
            //not an anonymous type. Its a "Type built in Runtime" using Reflection.Emit.

            //Prepare ParameterExpression refering to the source object
            var sourceItem = Expression.Parameter(source.ElementType, "t");

            //Get PropertyInfos from Source Object (Filter all Misspelled Property-Names)
            var sourceProperties = propertyNames.Where(name => source.ElementType.GetProperty(name) != null).ToDictionary(name => name, name => source.ElementType.GetProperty(name));

            //Build dynamic a Class that includes the Fields (no inheritance, no interfaces)
            var dynamicType = DynamicTypeBuilder.GetDynamicType(sourceProperties.Values.ToDictionary(f => f.Name, f => f.PropertyType));

            //Create the Binding Expressions
            var bindings = dynamicType.GetProperties().Where(p => p.CanWrite)
                .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>().ToList();

            //Create the Projection
            var selector = Expression.Lambda<Func<T, dynamic>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            //Now Select and return the IQueryable object
            return source.Select(selector);
        }

        // This function will not create a dynamic type, meaning it will return a list of objects that are the object we are querying
        // This is a temporary solution, Json.Net SUCKS at deserializing anonymous types, it just makes an expando which ServiceStack can't serialize
        // and Json.NET is the only option for NServiceBus ATM - in 5.0 they have an extension system setup to I can serialize using ServiceStack.
        // So until then the query will return a list of all fields, which I dont want.  I only want the user to know about the fields he/she has access to
        public static IQueryable<T> SelectPartialNoDynamic<T>(this IQueryable<T> source, IEnumerable<string> propertyNames)
        {
            Contract.Requires(source != null);

            //Prepare ParameterExpression refering to the source object
            var sourceItem = Expression.Parameter(source.ElementType, "t");

            //Get PropertyInfos from Source Object (Filter all Misspelled Property-Names)
            var sourceProperties = propertyNames.Where(name => source.ElementType.GetProperty(name) != null).ToDictionary(name => name, name => source.ElementType.GetProperty(name));

            var type = typeof(T);

            //Create the Binding Expressions
            var bindings = type.GetProperties().Where(p => p.CanWrite)
                .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>().ToList();

            //Create the Projection
            var selector = Expression.Lambda<Func<T, T>>(Expression.MemberInit(Expression.New(type.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            //Now Select and return the IQueryable object
            return source.Select(selector);
        }

        public static IQueryable<dynamic> SelectPartial<T>(this IQueryable<T> source, params string[] propertyNames)
        {
            return source.SelectPartial(propertyNames.AsEnumerable());
        }

        public static IQueryable<T> SelectPartialNoDynamic<T>(this IQueryable<T> source, params string[] propertyNames)
        {
            return source.SelectPartialNoDynamic(propertyNames.AsEnumerable());
        }


        public static dynamic ToPartial<T>(this T obj, IEnumerable<string> propertyNames)
        {
            Contract.Requires(obj != null);

            var objType = typeof(T);

            //Prepare ParameterExpression refering to the source object
            var sourceItem = Expression.Parameter(objType, "t");

            //Get PropertyInfos from Source Object (Filter all Misspelled Property-Names)
            var sourceProperties = propertyNames.Where(name => objType.GetProperty(name) != null).ToDictionary(name => name, name => objType.GetProperty(name));

            //Build dynamic a Class that includes the Fields (no inheritance, no interfaces)
            var dynamicType = DynamicTypeBuilder.GetDynamicType(sourceProperties.Values.ToDictionary(f => f.Name, f => f.PropertyType));

            //Create the Binding Expressions
            var bindings = dynamicType.GetProperties().Where(p => p.CanWrite)
                .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>().ToList();

            //Create the Projection
            var selector = Expression.Lambda<Func<T, dynamic>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            return selector.Compile().Invoke(obj);
        }

        public static dynamic ToPartial<T>(this T obj, params string[] propertyNames)
        {
            return obj.ToPartial(propertyNames.AsEnumerable());
        }

        public static IQueryable<T> SelectCondition<T>(this IQueryable<T> source, Func<PropertyInfo, bool> condition)
        {
            var fields = new List<string>();
            foreach (var prop in source.ElementType.GetProperties())
            {
                if (condition.Invoke(prop))
                    fields.Add(prop.Name);
            }
            return source.ToPartial(fields);
        }

        public static T ToCondition<T>(this T obj, Func<PropertyInfo, bool> condition)
        {
            var fields = new List<string>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (condition.Invoke(prop))
                    fields.Add(prop.Name);
            }
            return obj.ToPartial(fields);
        }
    }
}