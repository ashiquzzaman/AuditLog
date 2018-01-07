using AzR.AuditLog.DataAccess.AuditLog;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
//https://stackoverflow.com/questions/19797820/how-to-get-id-from-entity-for-auditlog-in-entity-framework-6
//https://stackoverflow.com/questions/20961489/how-to-create-an-audit-trail-with-entity-framework-5-and-mvc-4
namespace AzR.AuditLog.DataAccess.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AuditLog.AuditLog> AuditLogs { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }



        public override int SaveChanges()
        {
            using (var scope = new TransactionScope())
            {
                var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();

                foreach (var entry in modifiedEntries)
                {
                    //ApplyAuditLog(entry);
                }

                int changes = base.SaveChanges();
                foreach (var entry in addedEntries)
                {

                }

                base.SaveChanges();
                scope.Complete();
                return changes;
            }
        }

        private void ApplyAuditLog(DbEntityEntry entry)
        {
            ActionType operation;
            switch (entry.State)
            {
                case EntityState.Added:
                    operation = ActionType.Create;
                    break;
                case EntityState.Deleted:
                    operation = ActionType.Delete;
                    break;
                case EntityState.Modified:
                    operation = ActionType.Update;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //ApplyAuditLog(entry, operation);
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //  modelBuilder.Conventions.Add(new DateConvention());


            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles")
                .Property(c => c.Name).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users")
                .Property(c => c.UserName).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users")
                .Property(c => c.Email).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogins");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaims")
                .Property(u => u.ClaimType).HasMaxLength(150);

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaims")
                .Property(u => u.ClaimValue).HasMaxLength(500);



        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}