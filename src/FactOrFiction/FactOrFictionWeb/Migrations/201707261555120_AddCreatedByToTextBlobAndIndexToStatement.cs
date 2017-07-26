namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedByToTextBlobAndIndexToStatement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Statements", "IndexInParent", c => c.Int(nullable: false));
            AddColumn("dbo.TextBlobModels", "CreatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TextBlobModels", "CreatedBy");
            DropColumn("dbo.Statements", "IndexInParent");
        }
    }
}
