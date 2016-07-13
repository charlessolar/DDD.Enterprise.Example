using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Setup.Attributes
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
