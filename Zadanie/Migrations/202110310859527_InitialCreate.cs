namespace Zadanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCustomer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        StatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tblCustomerStatus", t => t.StatusID, cascadeDelete: true)
                .Index(t => t.StatusID);
            
            CreateTable(
                "dbo.tblCustomerStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tblEmail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmailContent = c.String(nullable: false),
                        Customer_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tblCustomer", t => t.Customer_ID)
                .Index(t => t.Customer_ID);
            
            CreateTable(
                "dbo.tblPhone",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PhoneContent = c.String(nullable: false),
                        Customer_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tblCustomer", t => t.Customer_ID)
                .Index(t => t.Customer_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblPhone", "Customer_ID", "dbo.tblCustomer");
            DropForeignKey("dbo.tblEmail", "Customer_ID", "dbo.tblCustomer");
            DropForeignKey("dbo.tblCustomer", "StatusID", "dbo.tblCustomerStatus");
            DropIndex("dbo.tblPhone", new[] { "Customer_ID" });
            DropIndex("dbo.tblEmail", new[] { "Customer_ID" });
            DropIndex("dbo.tblCustomer", new[] { "StatusID" });
            DropTable("dbo.tblPhone");
            DropTable("dbo.tblEmail");
            DropTable("dbo.tblCustomerStatus");
            DropTable("dbo.tblCustomer");
        }
    }
}
