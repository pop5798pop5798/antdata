﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SITW.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sitwEntities : DbContext
    {
        public sitwEntities()
            : base("name=sitwEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Assets> Assets { get; set; }
        public virtual DbSet<AssetsRecord> AssetsRecord { get; set; }
        public virtual DbSet<MissionEnd> MissionEnd { get; set; }
        public virtual DbSet<Missions> Missions { get; set; }
        public virtual DbSet<MissionStart> MissionStart { get; set; }
        public virtual DbSet<UserMissions> UserMissions { get; set; }
        public virtual DbSet<cfgMissionEnd> cfgMissionEnd { get; set; }
        public virtual DbSet<cfgMissionStart> cfgMissionStart { get; set; }
        public virtual DbSet<MissionAssets> MissionAssets { get; set; }
        public virtual DbSet<cfgUnit> cfgUnit { get; set; }
        public virtual DbSet<GlobalSetting> GlobalSetting { get; set; }
        public virtual DbSet<cfgVedio> cfgVedio { get; set; }
        public virtual DbSet<VedioRecord> VedioRecord { get; set; }
        public virtual DbSet<Leagues> Leagues { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<cfgLevelExp> cfgLevelExp { get; set; }
        public virtual DbSet<placard> placard { get; set; }
        public virtual DbSet<NewsMenu> NewsMenu { get; set; }
        public virtual DbSet<Seasons> Seasons { get; set; }
        public virtual DbSet<Ranking_content> Ranking_content { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<GlobalSettingRecord> GlobalSettingRecord { get; set; }
        public virtual DbSet<Rewards> Rewards { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SMSRecord> SMSRecord { get; set; }
        public virtual DbSet<UserClaims> UserClaims { get; set; }
        public virtual DbSet<UserLogins> UserLogins { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VerifyRecords> VerifyRecords { get; set; }
        public virtual DbSet<Ranking_title> Ranking_title { get; set; }
        public virtual DbSet<TransferRecords> TransferRecords { get; set; }
        public virtual DbSet<UserLoginRecord> UserLoginRecord { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UserToken> UserToken { get; set; }
        public virtual DbSet<APIPosts> APIPosts { get; set; }
        public virtual DbSet<ApiTypes> ApiTypes { get; set; }
        public virtual DbSet<ApiDataType> ApiDataType { get; set; }
    }
}
