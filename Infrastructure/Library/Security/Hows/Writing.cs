using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Hows
{
    public class Writing : IHow
    {
        private readonly IContainer _container;

        public Writing(IContainer container)
        {
            _container = container;
        }
        public String Description { get { return "Write"; } }
    }
}