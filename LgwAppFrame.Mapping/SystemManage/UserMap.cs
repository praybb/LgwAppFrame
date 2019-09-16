using LgwAppFrame.Domain.Entity.SystemManage;
using LgwAppFrame.EFDate;
using System.Data.Entity.ModelConfiguration;

namespace LgwAppFrame.Mapping.SystemManage
{
  public class UserMap :EntityTypeConfiguration<UserEntity>
    {
        public UserMap()
        {
            this.ToTable("Sys_User");
            this.HasKey(t => t.UUID_);
        }
       
    }
}
