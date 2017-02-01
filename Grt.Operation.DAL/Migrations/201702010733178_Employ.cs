namespace CMP.Operation.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Employ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "StartDate");
        }
    }
}
