namespace SITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddRegiDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RegistrationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RegistrationDate");
        }
    }
}
