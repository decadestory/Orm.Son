using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Mapper
{
    public class TableNameAttribute:Attribute
    {
        public string Name { get; set; }

        public TableNameAttribute(string name) {
            Name = name;
        }
    }
}
