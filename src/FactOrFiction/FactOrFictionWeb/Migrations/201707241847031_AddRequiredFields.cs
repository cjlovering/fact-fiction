namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.References", "CreatedBy", c => c.String(nullable: false));
            AlterColumn("dbo.References", "Link", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.References", "Link", c => c.String());
            AlterColumn("dbo.References", "CreatedBy", c => c.String());
        }
    }
}
