namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTextString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TextBlobModels", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TextBlobModels", "Text");
        }
    }
}
