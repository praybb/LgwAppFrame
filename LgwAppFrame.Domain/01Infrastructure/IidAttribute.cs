using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LgwAppFrame.Domain
{
    public interface IidAttribute
    {
        /// <summary>
        /// 自增性ID主键
        /// </summary>
        int ID { get; set; }
    }
}
