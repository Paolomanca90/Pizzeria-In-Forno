namespace U4_W6_D1_5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        NomeCliente = c.String(nullable: false, maxLength: 50),
                        CognomeCliente = c.String(nullable: false, maxLength: 50),
                        IndirizzoCliente = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Ordine",
                c => new
                    {
                        IdOrdine = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(),
                        DataOrdine = c.DateTime(),
                        StatoOrdine = c.String(maxLength: 50),
                        IndirizzoOrdine = c.String(maxLength: 50),
                        NoteOrdine = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdOrdine)
                .ForeignKey("dbo.Cliente", t => t.IdCliente)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "dbo.Dettaglio",
                c => new
                    {
                        IdDettaglio = c.Int(nullable: false, identity: true),
                        IdOrdine = c.Int(),
                        IdProdotto = c.Int(),
                        Quantita = c.Int(),
                    })
                .PrimaryKey(t => t.IdDettaglio)
                .ForeignKey("dbo.Ordine", t => t.IdOrdine)
                .ForeignKey("dbo.Prodotto", t => t.IdProdotto)
                .Index(t => t.IdOrdine)
                .Index(t => t.IdProdotto);
            
            CreateTable(
                "dbo.Prodotto",
                c => new
                    {
                        IdProdotto = c.Int(nullable: false, identity: true),
                        NomeProdotto = c.String(nullable: false, maxLength: 50),
                        FotoProdotto = c.String(nullable: false, maxLength: 50),
                        PrezzoProdotto = c.Decimal(nullable: false, storeType: "money"),
                        TempoConsegna = c.Int(nullable: false),
                        Ingredienti = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdProdotto);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.Utente",
                c => new
                    {
                        IdUtente = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdUtente);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dettaglio", "IdProdotto", "dbo.Prodotto");
            DropForeignKey("dbo.Dettaglio", "IdOrdine", "dbo.Ordine");
            DropForeignKey("dbo.Ordine", "IdCliente", "dbo.Cliente");
            DropIndex("dbo.Dettaglio", new[] { "IdProdotto" });
            DropIndex("dbo.Dettaglio", new[] { "IdOrdine" });
            DropIndex("dbo.Ordine", new[] { "IdCliente" });
            DropTable("dbo.Utente");
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.Prodotto");
            DropTable("dbo.Dettaglio");
            DropTable("dbo.Ordine");
            DropTable("dbo.Cliente");
        }
    }
}
