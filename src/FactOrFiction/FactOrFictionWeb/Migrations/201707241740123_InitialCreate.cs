namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextBlobModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Statements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Classification = c.Int(nullable: false),
                        TextBlobModel_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TextBlobModels", t => t.TextBlobModel_Id)
                .Index(t => t.TextBlobModel_Id);
            
            CreateTable(
                "dbo.References",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedBy = c.String(),
                        Rating = c.Int(nullable: false),
                        Link = c.String(),
                        Statement_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Statements", t => t.Statement_Id)
                .Index(t => t.Statement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statements", "TextBlobModel_Id", "dbo.TextBlobModels");
            DropForeignKey("dbo.References", "Statement_Id", "dbo.Statements");
            DropIndex("dbo.References", new[] { "Statement_Id" });
            DropIndex("dbo.Statements", new[] { "TextBlobModel_Id" });
            DropTable("dbo.References");
            DropTable("dbo.Statements");
            DropTable("dbo.TextBlobModels");
        }
    }
}
