using System;
using System.Linq;
using System.Data.Entity;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;

namespace LgwAppFrame.EFDate
{
    public class FEDbContext :DbContext
    {//
        public FEDbContext(): base(LgwAppFrame.Code.Configs.GetValue("datebaser"))
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        /// <summary>
        /// 查找映射程序集并加载
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //获得程序集的位置文件位置
            string assembleFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("LgwAppFrame.EFDate.DLL", "LgwAppFrame.Mapping.DLL").Replace("file:///", "");
            //加载程序集的内容
            Assembly asm = Assembly.LoadFile(assembleFileName);
            var typesToRegister = asm.GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);//加载模型
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
