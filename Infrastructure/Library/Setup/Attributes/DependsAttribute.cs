using System;

namespace Demo.Library.Setup.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependsAttribute : Attribute
    {
        public DependsAttribute(params string[] depends)
            : base()
        {
            this.Depends = depends;
        }

        public string[] Depends { get; set; }
    }
}
