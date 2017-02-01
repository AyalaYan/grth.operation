namespace CMP.Operation.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cmpDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        FirstFamilyName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        JobID = c.Int(nullable: false),
                        CountryID = c.Int(),
                        StateID = c.Int(),
                        CityID = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.City", t => t.CityID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .ForeignKey("dbo.Job", t => t.JobID, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.StateID)
                .Index(t => t.JobID)
                .Index(t => t.CountryID)
                .Index(t => t.StateID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StateID = c.Int(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.State", t => t.StateID, cascadeDelete: true)
                .Index(t => t.StateID);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountryID = c.Int(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Country", t => t.CountryID, cascadeDelete: true)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ShortName = c.String(),
                        Name = c.String(),
                        IsSystem = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.FocalPoint",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        CountryID = c.Int(),
                        StateID = c.Int(),
                        CityID = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.City", t => t.CityID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.StateID)
                .Index(t => t.CustomerID)
                .Index(t => t.CountryID)
                .Index(t => t.StateID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.ProjectType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Technology",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CustomerID = c.Int(nullable: false),
                        DepartmentID = c.Int(),
                        ProjectTypeID = c.Int(nullable: false),
                        FocalPointID = c.Int(),
                        CompanyFocalPointID = c.Int(),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Risk = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.CompanyFocalPointID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Department", t => t.DepartmentID)
                .ForeignKey("dbo.FocalPoint", t => t.FocalPointID)
                .ForeignKey("dbo.ProjectType", t => t.ProjectTypeID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.DepartmentID)
                .Index(t => t.ProjectTypeID)
                .Index(t => t.FocalPointID)
                .Index(t => t.CompanyFocalPointID);
            
            CreateTable(
                "dbo.ProjectTechnology",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        TechnologyID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.TechnologyID })
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Technology", t => t.TechnologyID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.TechnologyID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Experience",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => new { t.EmployeeID, t.CompanyID, t.FromDate }, unique: true, name: "IX_Experience_Employee_Company");
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CustomerID = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.ExperienceTechnology",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExperienceID = c.Int(nullable: false),
                        TechnologyID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ExperienceID, t.TechnologyID })
                .ForeignKey("dbo.Experience", t => t.ExperienceID, cascadeDelete: true)
                .ForeignKey("dbo.Technology", t => t.TechnologyID, cascadeDelete: true)
                .Index(t => t.ExperienceID)
                .Index(t => t.TechnologyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExperienceTechnology", "TechnologyID", "dbo.Technology");
            DropForeignKey("dbo.ExperienceTechnology", "ExperienceID", "dbo.Experience");
            DropForeignKey("dbo.Experience", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.Experience", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Company", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.User", "RoleID", "dbo.Role");
            DropForeignKey("dbo.ProjectTechnology", "TechnologyID", "dbo.Technology");
            DropForeignKey("dbo.ProjectTechnology", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Project", "ProjectTypeID", "dbo.ProjectType");
            DropForeignKey("dbo.Project", "FocalPointID", "dbo.FocalPoint");
            DropForeignKey("dbo.Project", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Project", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.Project", "CompanyFocalPointID", "dbo.Employee");
            DropForeignKey("dbo.FocalPoint", "StateID", "dbo.State");
            DropForeignKey("dbo.FocalPoint", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.FocalPoint", "CountryID", "dbo.Country");
            DropForeignKey("dbo.FocalPoint", "CityID", "dbo.City");
            DropForeignKey("dbo.Department", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.Employee", "StateID", "dbo.State");
            DropForeignKey("dbo.Employee", "JobID", "dbo.Job");
            DropForeignKey("dbo.Employee", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Employee", "CityID", "dbo.City");
            DropForeignKey("dbo.City", "StateID", "dbo.State");
            DropForeignKey("dbo.State", "CountryID", "dbo.Country");
            DropIndex("dbo.ExperienceTechnology", new[] { "TechnologyID" });
            DropIndex("dbo.ExperienceTechnology", new[] { "ExperienceID" });
            DropIndex("dbo.Company", new[] { "CustomerID" });
            DropIndex("dbo.Experience", "IX_Experience_Employee_Company");
            DropIndex("dbo.User", new[] { "RoleID" });
            DropIndex("dbo.ProjectTechnology", new[] { "TechnologyID" });
            DropIndex("dbo.ProjectTechnology", new[] { "ProjectID" });
            DropIndex("dbo.Project", new[] { "CompanyFocalPointID" });
            DropIndex("dbo.Project", new[] { "FocalPointID" });
            DropIndex("dbo.Project", new[] { "ProjectTypeID" });
            DropIndex("dbo.Project", new[] { "DepartmentID" });
            DropIndex("dbo.Project", new[] { "CustomerID" });
            DropIndex("dbo.FocalPoint", new[] { "CityID" });
            DropIndex("dbo.FocalPoint", new[] { "StateID" });
            DropIndex("dbo.FocalPoint", new[] { "CountryID" });
            DropIndex("dbo.FocalPoint", new[] { "CustomerID" });
            DropIndex("dbo.Department", new[] { "CustomerID" });
            DropIndex("dbo.State", new[] { "CountryID" });
            DropIndex("dbo.City", new[] { "StateID" });
            DropIndex("dbo.Employee", new[] { "CityID" });
            DropIndex("dbo.Employee", new[] { "StateID" });
            DropIndex("dbo.Employee", new[] { "CountryID" });
            DropIndex("dbo.Employee", new[] { "JobID" });
            DropTable("dbo.ExperienceTechnology");
            DropTable("dbo.Company");
            DropTable("dbo.Experience");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.ProjectTechnology");
            DropTable("dbo.Project");
            DropTable("dbo.Technology");
            DropTable("dbo.ProjectType");
            DropTable("dbo.FocalPoint");
            DropTable("dbo.Department");
            DropTable("dbo.Customer");
            DropTable("dbo.Job");
            DropTable("dbo.Country");
            DropTable("dbo.State");
            DropTable("dbo.City");
            DropTable("dbo.Employee");
        }
    }
}
