namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.References",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        Tags = c.String(nullable: false),
                        Link = c.String(nullable: false),
                        Statement_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Statements", t => t.Statement_Id)
                .Index(t => t.Statement_Id);
            
            CreateTable(
                "dbo.Statements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        Classification = c.Int(nullable: false),
                        TextBlobModel_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TextBlobModels", t => t.TextBlobModel_Id)
                .Index(t => t.TextBlobModel_Id);
            
            CreateTable(
                "dbo.TextBlobModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statements", "TextBlobModel_Id", "dbo.TextBlobModels");
            DropForeignKey("dbo.References", "Statement_Id", "dbo.Statements");
            DropIndex("dbo.Statements", new[] { "TextBlobModel_Id" });
            DropIndex("dbo.References", new[] { "Statement_Id" });
            DropTable("dbo.TextBlobModels");
            DropTable("dbo.Statements");
            DropTable("dbo.References");
        }
    }
}
