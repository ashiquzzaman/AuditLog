using AzR.AuditLog.DataAccess.AuditLog;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

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
                    var audit = CreateLog.Create(entry);
                    AuditLogs.Add(audit);
                }
                int changes = base.SaveChanges();
                foreach (var entry in addedEntries)
                {
                    var audit = CreateLog.Create(entry);
                    AuditLogs.Add(audit);

                }

                base.SaveChanges();
                scope.Complete();
                return changes;
            }
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