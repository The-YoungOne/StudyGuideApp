namespace StudyWithMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleCalendars",
                c => new
                    {
                        calendar_id = c.Int(nullable: false, identity: true),
                        studyDate = c.DateTime(nullable: false),
                        hoursStudied = c.Double(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        Module_module_id = c.Int(),
                    })
                .PrimaryKey(t => t.calendar_id)
                .ForeignKey("dbo.Modules", t => t.Module_module_id)
                .Index(t => t.Module_module_id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        module_id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false),
                        name = c.String(nullable: false),
                        credits = c.Int(nullable: false),
                        classHrsPerWeek = c.Int(nullable: false),
                        Username_username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.module_id)
                .ForeignKey("dbo.Users", t => t.Username_username)
                .Index(t => t.Username_username);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        username = c.String(nullable: false, maxLength: 128),
                        password = c.String(nullable: false),
                        name = c.String(nullable: false),
                        surname = c.String(nullable: false),
                        email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.username);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        semester_id = c.Int(nullable: false, identity: true),
                        weeks = c.Int(nullable: false),
                        startDate = c.DateTime(nullable: false),
                        endDate = c.DateTime(nullable: false),
                        Username_username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.semester_id)
                .ForeignKey("dbo.Users", t => t.Username_username)
                .Index(t => t.Username_username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Semesters", "Username_username", "dbo.Users");
            DropForeignKey("dbo.Modules", "Username_username", "dbo.Users");
            DropForeignKey("dbo.ModuleCalendars", "Module_module_id", "dbo.Modules");
            DropIndex("dbo.Semesters", new[] { "Username_username" });
            DropIndex("dbo.Modules", new[] { "Username_username" });
            DropIndex("dbo.ModuleCalendars", new[] { "Module_module_id" });
            DropTable("dbo.Semesters");
            DropTable("dbo.Users");
            DropTable("dbo.Modules");
            DropTable("dbo.ModuleCalendars");
        }
    }
}
