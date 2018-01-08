using AzR.AuditLog.DataAccess.AuditLog;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
            try
            {
                using (var scope = new TransactionScope())
                {
                    var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                    var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
                    var deleteEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

                    var changes = 0;
                    if (deleteEntries.Count > 0)
                    {
                        foreach (var entry in deleteEntries)
                        {
                            var audit = CreateLog.Create(entry, 3);
                            AuditLogs.Add(audit);
                        }
                        changes = base.SaveChanges();
                    }

                    if (modifiedEntries.Count > 0)
                    {
                        foreach (var entry in modifiedEntries)
                        {
                            var audit = CreateLog.Create(entry, 2);
                            AuditLogs.Add(audit);
                        }
                        changes = base.SaveChanges();
                    }

                    if (addedEntries.Count > 0)
                    {

                        foreach (var entry in addedEntries)
                        {
                            var audit = CreateLog.Create(entry, 1);
                            AuditLogs.Add(audit);
                        }

                        changes = base.SaveChanges();
                    }

                    scope.Complete();
                    return changes;
                }

            }
            catch (DbEntityValidationException ex)
            {
                var outputLines = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //GeneralHelper.WriteValue(string.Join("\n", outputLines));
                throw new Exception(string.Join(",", outputLines.ToArray()));

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