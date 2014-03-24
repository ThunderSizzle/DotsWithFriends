namespace DotsWithFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserLogins2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUserLogins", "ProviderKey", c => c.String(nullable: false, maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUserLogins", "ProviderKey", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
