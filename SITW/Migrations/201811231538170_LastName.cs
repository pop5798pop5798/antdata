namespace SITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastName");
        }
    }
}
