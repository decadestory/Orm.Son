using Orm.Son.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Core.Test.Entities
{
    [TableName("Man")]
    public class User
    {
        [Description("编号")]
        public int Id { get; set; }
        [Description("名称")]
        public string Name { get; set; }
        public int? Age { get; set; }
        public int Score { get; set; }
        public DateTime AddTime { get; set; }
    }
}
