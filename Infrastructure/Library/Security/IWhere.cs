using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IWhere<T>
    {
        String Description { get; set; }
        Boolean Authorized(T instance);
    }

    // A where based on the Who requesting
    public interface IWhereWho<T, TWho> : IWhere<T> where TWho : IWho
    {
    }

    // A where based on the What being requested
    public interface IWhereWhat<T, TWhat> : IWhere<T> where TWhat : IWhat
    {
    }
}