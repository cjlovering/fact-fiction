namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTagsAndCreatedByNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.References", "CreatedBy", c => c.String());
            AlterColumn("dbo.References", "Tags", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.References", "Tags", c => c.String(nullable: false));
            AlterColumn("dbo.References", "CreatedBy", c => c.String(nullable: false));
        }
    }
}
