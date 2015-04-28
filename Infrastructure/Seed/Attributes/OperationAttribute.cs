using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationAttribute : Attribute
    {
        public OperationAttribute(String Name)
            : base()
        {
            this.Name = Name;
        }

        public String Name { get; set; }
    }
}