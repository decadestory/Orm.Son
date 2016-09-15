using Orm.Son.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Test.Entities
{
    public class Demo2
    {
		[Key]
        public int MyId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Score { get; set; }
        public DateTime AddTime { get; set; }
        public bool IsDel { get; set; }
    }
}
