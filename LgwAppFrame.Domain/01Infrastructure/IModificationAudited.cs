using System;

namespace LgwAppFrame.Domain
{
    /// <summary>
    /// 修改审计属性接口
    /// </summary>
    public interface IModificationAudited
    {
    
        /// <summary>
        /// 修改用户
        /// </summary>
        int? MID_ { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? MDATE_ { get; set; }
    }
}
