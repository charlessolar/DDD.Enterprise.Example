using Demo.Library.Dynamics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        public static IQueryable<dynamic> SelectPartial<T>(this IQueryable<T> source, IEnumerable<String> propertyNames)
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
            var dynamicType = DynamicTypeBuilder.GetDynamicType(sourceProperties.Values.ToDictionary(f => f.Name, f => f.PropertyType), typeof(object), Type.EmptyTypes);

            //Create the Binding Expressions
            var bindings = dynamicType.GetProperties().Where(p => p.CanWrite)
                .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>().ToList();

            //Create the Projection
            var selector = Expression.Lambda<Func<T, dynamic>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            //Now Select and return the IQueryable object
            return source.Select(selector);
        }

        public static dynamic ToPartial<T>(this T obj, IEnumerable<String> propertyNames)
        {
            Contract.Requires(obj != null);

            var objType = typeof(T);

            //Prepare ParameterExpression refering to the source object
            var sourceItem = Expression.Parameter(objType, "t");

            //Get PropertyInfos from Source Object (Filter all Misspelled Property-Names)
            var sourceProperties = propertyNames.Where(name => objType.GetProperty(name) != null).ToDictionary(name => name, name => objType.GetProperty(name));

            //Build dynamic a Class that includes the Fields (no inheritance, no interfaces)
            var dynamicType = DynamicTypeBuilder.GetDynamicType(sourceProperties.Values.ToDictionary(f => f.Name, f => f.PropertyType), typeof(object), Type.EmptyTypes);

            //Create the Binding Expressions
            var bindings = dynamicType.GetProperties().Where(p => p.CanWrite)
                .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>().ToList();

            //Create the Projection
            var selector = Expression.Lambda<Func<T, dynamic>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            return selector.Compile().Invoke(obj);
        }
    }
}