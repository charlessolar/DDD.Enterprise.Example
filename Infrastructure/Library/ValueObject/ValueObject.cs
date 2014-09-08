using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.ValueObject
{
    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        private List<PropertyInfo> Properties { get; set; }

        protected ValueObject()
        {
            Properties = new List<PropertyInfo>();
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as T);
        }

        public Boolean Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            foreach (var property in Properties)
            {
                var oneValue = property.GetValue(this, null);
                var otherValue = property.GetValue(other, null);

                if (null == oneValue || null == otherValue) return false;
                if (false == oneValue.Equals(otherValue)) return false;
            }

            return true;
        }

        public override Int32 GetHashCode()
        {
            var hashCode = 36;
            foreach (var property in Properties)
            {
                var propertyValue = property.GetValue(this, null);
                if (null == propertyValue)
                    continue;

                hashCode = hashCode ^ propertyValue.GetHashCode();
            }

            return hashCode;
        }

        public override String ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var property in Properties)
            {
                var propertyValue = property.GetValue(this, null);
                if (null == propertyValue)
                    continue;

                stringBuilder.Append(propertyValue.ToString());
            }

            return stringBuilder.ToString();
        }

        protected void RegisterProperty(
            Expression<Func<T, Object>> expression)
        {
            Contract.Requires(expression != null);

            MemberExpression memberExpression;
            if (ExpressionType.Convert == expression.Body.NodeType)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (null == memberExpression)
                throw new InvalidOperationException();

            Properties.Add(memberExpression.Member as PropertyInfo);
        }
    }
}
