namespace DotsWithFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatabaseObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Boxes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        East_Id = c.Guid(),
                        North_Id = c.Guid(),
                        NorthEast_Id = c.Guid(),
                        NorthWest_Id = c.Guid(),
                        Owner_Id = c.Guid(),
                        South_Id = c.Guid(),
                        SouthEast_Id = c.Guid(),
                        SouthWest_Id = c.Guid(),
                        West_Id = c.Guid(),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .ForeignKey("dbo.Lines", t => t.East_Id)
                .ForeignKey("dbo.Lines", t => t.North_Id)
                .ForeignKey("dbo.Coordinates", t => t.NorthEast_Id)
                .ForeignKey("dbo.Coordinates", t => t.NorthWest_Id)
                .ForeignKey("dbo.Players", t => t.Owner_Id)
                .ForeignKey("dbo.Lines", t => t.South_Id)
                .ForeignKey("dbo.Coordinates", t => t.SouthEast_Id)
                .ForeignKey("dbo.Coordinates", t => t.SouthWest_Id)
                .ForeignKey("dbo.Lines", t => t.West_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Id)
                .Index(t => t.East_Id)
                .Index(t => t.North_Id)
                .Index(t => t.NorthEast_Id)
                .Index(t => t.NorthWest_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.South_Id)
                .Index(t => t.SouthEast_Id)
                .Index(t => t.SouthWest_Id)
                .Index(t => t.West_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Coordinates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        width = c.Int(nullable: false),
                        height = c.Int(nullable: false),
                        CurrentPlayer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Grids",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        width = c.Int(nullable: false),
                        height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        From_Id = c.Guid(),
                        Player_Id = c.Guid(),
                        To_Id = c.Guid(),
                        Created = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .ForeignKey("dbo.Coordinates", t => t.From_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .ForeignKey("dbo.Coordinates", t => t.To_Id)
                .Index(t => t.Id)
                .Index(t => t.From_Id)
                .Index(t => t.Player_Id)
                .Index(t => t.To_Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Profile_Id = c.Guid(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Turn",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Line_Id = c.Guid(),
                        Player_Id = c.Guid(),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DatabaseObjects", t => t.Id)
                .ForeignKey("dbo.Lines", t => t.Line_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Id)
                .Index(t => t.Line_Id)
                .Index(t => t.Player_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turn", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Turn", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Turn", "Line_Id", "dbo.Lines");
            DropForeignKey("dbo.Turn", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Profiles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Profiles", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Players", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Players", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Lines", "To_Id", "dbo.Coordinates");
            DropForeignKey("dbo.Lines", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Lines", "From_Id", "dbo.Coordinates");
            DropForeignKey("dbo.Lines", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Grids", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Games", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Coordinates", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.Boxes", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Boxes", "West_Id", "dbo.Lines");
            DropForeignKey("dbo.Boxes", "SouthWest_Id", "dbo.Coordinates");
            DropForeignKey("dbo.Boxes", "SouthEast_Id", "dbo.Coordinates");
            DropForeignKey("dbo.Boxes", "South_Id", "dbo.Lines");
            DropForeignKey("dbo.Boxes", "Owner_Id", "dbo.Players");
            DropForeignKey("dbo.Boxes", "NorthWest_Id", "dbo.Coordinates");
            DropForeignKey("dbo.Boxes", "NorthEast_Id", "dbo.Coordinates");
            DropForeignKey("dbo.Boxes", "North_Id", "dbo.Lines");
            DropForeignKey("dbo.Boxes", "East_Id", "dbo.Lines");
            DropForeignKey("dbo.Boxes", "Id", "dbo.DatabaseObjects");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.Users");
            DropIndex("dbo.Turn", new[] { "Game_Id" });
            DropIndex("dbo.Turn", new[] { "Player_Id" });
            DropIndex("dbo.Turn", new[] { "Line_Id" });
            DropIndex("dbo.Turn", new[] { "Id" });
            DropIndex("dbo.Profiles", new[] { "User_Id" });
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropIndex("dbo.Players", new[] { "Profile_Id" });
            DropIndex("dbo.Players", new[] { "Id" });
            DropIndex("dbo.Lines", new[] { "To_Id" });
            DropIndex("dbo.Lines", new[] { "Player_Id" });
            DropIndex("dbo.Lines", new[] { "From_Id" });
            DropIndex("dbo.Lines", new[] { "Id" });
            DropIndex("dbo.Grids", new[] { "Id" });
            DropIndex("dbo.Games", new[] { "Id" });
            DropIndex("dbo.Coordinates", new[] { "Id" });
            DropIndex("dbo.Boxes", new[] { "Game_Id" });
            DropIndex("dbo.Boxes", new[] { "West_Id" });
            DropIndex("dbo.Boxes", new[] { "SouthWest_Id" });
            DropIndex("dbo.Boxes", new[] { "SouthEast_Id" });
            DropIndex("dbo.Boxes", new[] { "South_Id" });
            DropIndex("dbo.Boxes", new[] { "Owner_Id" });
            DropIndex("dbo.Boxes", new[] { "NorthWest_Id" });
            DropIndex("dbo.Boxes", new[] { "NorthEast_Id" });
            DropIndex("dbo.Boxes", new[] { "North_Id" });
            DropIndex("dbo.Boxes", new[] { "East_Id" });
            DropIndex("dbo.Boxes", new[] { "Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.Turn");
            DropTable("dbo.Profiles");
            DropTable("dbo.Players");
            DropTable("dbo.Lines");
            DropTable("dbo.Grids");
            DropTable("dbo.Games");
            DropTable("dbo.Coordinates");
            DropTable("dbo.Boxes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.DatabaseObjects");
        }
    }
}
