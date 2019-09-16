using LgwAppFrame.Code;
using LgwAppFrame.EFDate;
using System;

namespace LgwAppFrame.Domain
{
    /// <summary>
    /// 审计实体接口
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public class IEntity<TEntity>
    {
        /// <summary>
        /// 审计创建方法
        /// </summary>
        public virtual void Create()
        {
            Create(DateTime.Now);
        }
        /// <summary>
        /// 审计创建方法
        /// </summary>
        /// <param name="time"></param>
        public void Create(DateTime time)
        {
            var entity = this as ICreationAudited;
            entity.CDATE_ = time;
            if (Array.IndexOf(this.GetType().GetInterfaces(), typeof(IuuidAttribute)) > -1)
            {
                var entity2 = entity as IuuidAttribute;
                entity2.UUID_ = Common.GuId();             
            }





            //var LoginInfo = OperatorProvider.Provider.GetCurrent();
            //if (LoginInfo != null)
            //{
            //    entity.CID_ = LoginInfo.UserId;
            //}
            
        }
        /// <summary>
        /// 审计修改方法
        /// </summary>
        /// <param name="keyValue">F_Id主键</param>
        public void Modify(string keyValue)
        {
            Modify(keyValue, DateTime.Now);
        }
        /// <summary>
        /// 审计修改方法
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="time"></param>
        public void Modify(string keyValue, DateTime time)
        {
            //var entity = this as IModificationAudited;
            //entity.F_Id = keyValue;
            //var LoginInfo = OperatorProvider.Provider.GetCurrent();
            //if (LoginInfo != null)
            //{
            //    entity.F_LastModifyUserId = LoginInfo.UserId;
            //}
            //entity.F_LastModifyTime = time;
        }
        /// <summary>
        /// 审计删除方法
        /// </summary>
        public void Remove()
        {
            //var entity = this as IDeleteAudited;
            //var LoginInfo = OperatorProvider.Provider.GetCurrent();
            //if (LoginInfo != null)
            //{
            //    entity.F_DeleteUserId = LoginInfo.UserId;
            //}
            //entity.F_DeleteTime = DateTime.Now;
            //entity.F_DeleteMark = true;
        }
    }
}
