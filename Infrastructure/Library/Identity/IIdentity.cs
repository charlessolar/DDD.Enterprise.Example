using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Identity
{
    // An class for designating an object as a POCO for aggregate or entity identity
    // It contains their business definition
    public interface IIdentity
    {
        Guid Id { get; set; }
    }

    public class Identity<T> : IIdentity, IEquatable<T> where T : IIdentity
    {
        public Guid Id { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public bool Equals(T other)
        {
            return this.Id.Equals(other.Id);
        }

        public override string ToString()
        {
            return this.Id.ToString("N");
        }
    }
}