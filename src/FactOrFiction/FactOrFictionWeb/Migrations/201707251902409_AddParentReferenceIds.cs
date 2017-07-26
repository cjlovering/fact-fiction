namespace FactOrFictionWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParentReferenceIds : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.References", name: "Statement_Id", newName: "StatementId");
            RenameColumn(table: "dbo.Statements", name: "TextBlobModel_Id", newName: "TextBlobModelId");
            RenameIndex(table: "dbo.References", name: "IX_Statement_Id", newName: "IX_StatementId");
            RenameIndex(table: "dbo.Statements", name: "IX_TextBlobModel_Id", newName: "IX_TextBlobModelId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Statements", name: "IX_TextBlobModelId", newName: "IX_TextBlobModel_Id");
            RenameIndex(table: "dbo.References", name: "IX_StatementId", newName: "IX_Statement_Id");
            RenameColumn(table: "dbo.Statements", name: "TextBlobModelId", newName: "TextBlobModel_Id");
            RenameColumn(table: "dbo.References", name: "StatementId", newName: "Statement_Id");
        }
    }
}
