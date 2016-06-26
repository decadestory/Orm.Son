using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Mapper
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
