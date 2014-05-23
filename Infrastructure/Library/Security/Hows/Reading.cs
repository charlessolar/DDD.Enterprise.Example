using Castle.DynamicProxy;
using Demo.Library.Exceptions;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Hows
{
    public class Reading : IHow
    {
        private readonly IContainer _container;
        private IList<IWhat> _whats;

        public Reading(IContainer container)
        {
            _container = container;
            _whats = new List<IWhat>();
        }
        public String Description { get; set; }
        public void AddWhat(IWhat what)
        {
            _whats.Add(what);
        }
    }
}