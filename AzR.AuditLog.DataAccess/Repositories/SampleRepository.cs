using AzR.AuditLog.DataAccess.Entities;
using System.Data.Entity;

namespace AzR.AuditLog.DataAccess.Repositories
{
    public class SampleRepository : Repository<Sample>, ISampleRepository
    {
        public SampleRepository(DbContext context) : base(context)
        {
        }
    }
}
