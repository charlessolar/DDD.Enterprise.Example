using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IRequest
    {
        Tuple<String, String> Who { get; }
        Tuple<String, String> What { get; }
        Tuple<String, String> How { get; }
    }
}