using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Test.Entities
{
    public class Demo3
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public int Score { get; set; }
        public string AddTime { get; set; }
        public int? IsDel { get; set; }
    }

    public class Demo4
    {
        public int Id2 { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int? Score { get; set; }
        public DateTime? AddTime { get; set; }
        public int? IsDel { get; set; }
        public bool? IsDel2 { get; set; }
        public int? IsDel3 { get; set; }
    }
}
