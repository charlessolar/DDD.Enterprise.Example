using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Builders
{
    public class Builder : IBuilder
    {
        public Builder(IDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public IDescriptor Descriptor { get; private set; }
    }
}