using System;

namespace LgwAppFrame.Domain
{
    /// <summary>
    /// 逻辑删除属性接口
    /// </summary>
    public interface IDeleteAudited
    {
        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        bool? isDelete { get; set; }

        /// <summary>
        /// 删除实体的用户
        /// </summary>
        string DID_ { get; set; }

        /// <summary>
        /// 删除实体时间
        /// </summary>
        DateTime? DDATE_ { get; set; }
    }
}
