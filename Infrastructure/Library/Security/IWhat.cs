using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IWhat
    {
        String Description { get; }

        void Authorize();
    }
}