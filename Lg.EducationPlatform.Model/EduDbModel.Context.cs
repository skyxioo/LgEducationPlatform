﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lg.EducationPlatform.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EduDb4LgEntities : DbContext
    {
        public EduDb4LgEntities()
            : base("name=EduDb4LgEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WebSettings> WebSettings { get; set; }
    }
}
