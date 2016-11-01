using System;

namespace Demo.Library.Setup.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string name)
            : base()
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
