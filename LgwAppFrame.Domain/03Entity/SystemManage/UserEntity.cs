using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LgwAppFrame.Domain.Entity.SystemManage
{
   public class UserEntity : IEntity<UserEntity>,ICreationAudited,IModificationAudited,IuuidAttribute
    {       
        #region 实现接口
        public string UUID_ { get; set; }
        public int? CID_ { get; set; }
        public DateTime? CDATE_ { get; set; }
        public int? MID_ { get; set; }
        public DateTime? MDATE_ { get; set; }
        #endregion
        /// <summary>
        /// 帐号
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户别名
        /// </summary>
        public string userno { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string displayName { get; set; }
        public string email { get; set; }
        /// <summary>
        /// 工作电话
        /// </summary>
        public string phoneNum { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string mobilePhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 软件版本号
        /// </summary>
        public string softVersion { get; set; }
        /// <summary>
        /// 锁定
        /// </summary>
        public uint? isLock { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int? deptId { get; set; }
        /// <summary>
        /// 主要职位
        /// </summary>
        public int? firstJobId { get; set; }
        /// <summary>
        /// 次要职位
        /// </summary>
        public int? secondJobId { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int? orderNum { get; set; }

    }
}
