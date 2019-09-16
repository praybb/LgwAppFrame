using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LgwAppFrame.Domain
{
   public interface IuuidAttribute
    {
        /// <summary>
        /// 唯一性ID
        /// </summary>
        string UUID_ { get; set; }
    }
}
