using System.Data.Entity;

namespace AzR.AuditLog.DataAccess.Repositories
{
    public class AuditLogRepository : Repository<AuditLog.AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(DbContext context) : base(context)
        {
        }
    }
}