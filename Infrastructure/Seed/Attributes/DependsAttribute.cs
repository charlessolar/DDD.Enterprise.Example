using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependsAttribute : Attribute
    {
        public DependsAttribute(params String[] Depends)
            : base()
        {
            this.Depends = Depends;
        }

        public String[] Depends { get; set; }
    }
}
