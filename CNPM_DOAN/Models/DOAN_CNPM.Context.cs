﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CNPM_DOAN.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CNPM_DOANEntities : DbContext
    {
        public CNPM_DOANEntities()
            : base("name=CNPM_DOANEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BAIGIAI> BAIGIAIs { get; set; }
        public virtual DbSet<BAITAP> BAITAPs { get; set; }
        public virtual DbSet<MUCTIEU> MUCTIEUx { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public virtual DbSet<THOIKHOABIEU> THOIKHOABIEUx { get; set; }
        public virtual DbSet<TIETHOC> TIETHOCs { get; set; }
        public virtual DbSet<TODO> TODOes { get; set; }
        public virtual DbSet<VAITRO> VAITROes { get; set; }
        public virtual DbSet<TAILIEU> TAILIEUx { get; set; }
    }
}