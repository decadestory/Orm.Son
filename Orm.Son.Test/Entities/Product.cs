using Orm.Son.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Test.Entities
{
    [TableName("product")]
    public class Product
    {
        [Description("产品Id")]
        public int Id { get; set; }

        [Description("产品名称")]
        public string Name { get; set; }

        [Description("产品线Id")]
        public int? ProductLine { get; set; }

        [Description("添加时间")]
        public DateTime AddTime { get; set; }

        [Description("是否启用")]
        public bool Enable { get; set; }
    }
}
