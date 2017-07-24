namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatementTextField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Statements", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Statements", "Text");
        }
    }
}
