using AzR.AuditLog.DataAccess.Entities;
using System.Data.Entity.Migrations;

namespace AzR.AuditLog.DataAccess.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(ApplicationDbContext context)
        {

        }
    }
}
