using System;

namespace Demo.Library.Setup.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationAttribute : Attribute
    {
        public OperationAttribute(string name)
            : base()
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}