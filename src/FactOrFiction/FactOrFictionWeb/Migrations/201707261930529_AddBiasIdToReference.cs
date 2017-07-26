namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBiasIdToReference : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.References", name: "Bias_Id", newName: "BiasId");
            RenameIndex(table: "dbo.References", name: "IX_Bias_Id", newName: "IX_BiasId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.References", name: "IX_BiasId", newName: "IX_Bias_Id");
            RenameColumn(table: "dbo.References", name: "BiasId", newName: "Bias_Id");
        }
    }
}
