namespace U4_W6_D1_5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FotoProdottoProdotto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prodotto", "FotoProdotto", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prodotto", "FotoProdotto", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
