﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Insurance_сompany
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InsuranceCompanyEntities : DbContext
    {
        public InsuranceCompanyEntities()
            : base("name=InsuranceCompanyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Insurance_Manager> Insurance_Manager { get; set; }
        public virtual DbSet<Insurance_Policy> Insurance_Policy { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Statuses> Statuses { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Type_Insurance> Type_Insurance { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_Type> User_Type { get; set; }
        public virtual DbSet<UserBank> UserBank { get; set; }
        public virtual DbSet<Individual_User> Individual_User { get; set; }
        public virtual DbSet<Legal_User> Legal_User { get; set; }
    }
}