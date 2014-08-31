using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Responses
{
    public interface IIsList<T> where T : IHasGuidId
    {
        IEnumerable<T> Results { get; set; }
    }
}
