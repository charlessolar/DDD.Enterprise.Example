using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Setup
{
    public interface ISetup
    {
        Boolean Initialize();

        Boolean Done { get; }
    }
}
