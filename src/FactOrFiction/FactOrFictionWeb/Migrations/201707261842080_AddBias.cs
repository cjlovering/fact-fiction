namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bias",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Source = c.String(),
                        Factuality = c.String(),
                        BiasType = c.String(),
                        Notes = c.String(),
                        ReferenceId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.References", "Bias_Id", c => c.Guid());
            CreateIndex("dbo.References", "Bias_Id");
            AddForeignKey("dbo.References", "Bias_Id", "dbo.Bias", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.References", "Bias_Id", "dbo.Bias");
            DropIndex("dbo.References", new[] { "Bias_Id" });
            DropColumn("dbo.References", "Bias_Id");
            DropTable("dbo.Bias");
        }
    }
}
