namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Entities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedBy = c.String(),
                        Name = c.String(),
                        WikiUrl = c.String(),
                        TextBlobModelId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TextBlobModels", t => t.TextBlobModelId)
                .Index(t => t.TextBlobModelId);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                        Offset = c.Int(nullable: false),
                        EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entities", t => t.EntityId)
                .Index(t => t.EntityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entities", "TextBlobModelId", "dbo.TextBlobModels");
            DropForeignKey("dbo.Matches", "EntityId", "dbo.Entities");
            DropIndex("dbo.Matches", new[] { "EntityId" });
            DropIndex("dbo.Entities", new[] { "TextBlobModelId" });
            DropTable("dbo.Matches");
            DropTable("dbo.Entities");
        }
    }
}
