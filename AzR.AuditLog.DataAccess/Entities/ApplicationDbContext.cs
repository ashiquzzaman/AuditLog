using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

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