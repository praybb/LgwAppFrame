using System;

namespace LgwAppFrame.Domain
{
    /// <summary>
    /// 创建审计属性接口
    /// </summary>
    public interface ICreationAudited
    {
   
        /// <summary>
        /// 创建人ID
        /// </summary>
        int? CID_ { get; set; }
        /// <summary>
        /// 创建时间 
        /// </summary>
        DateTime? CDATE_ { get; set; }
    }
}
