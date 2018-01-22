using System;

namespace Orm.Son.Core.Mapper
{
    public class DescriptionAttribute: Attribute
    {
        public string Name { get; set; }

        public DescriptionAttribute(string name)
        {
            Name = name;
        }
    }
}
