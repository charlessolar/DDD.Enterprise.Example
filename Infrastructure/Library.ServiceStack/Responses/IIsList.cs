using ServiceStack.Model;
using System.Collections.Generic;

namespace Demo.Library.Responses
{
    public interface IIsList<T> where T : IHasGuidId
    {
        IEnumerable<T> Results { get; set; }
    }
}