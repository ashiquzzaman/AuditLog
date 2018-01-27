using AzR.AuditLog.Business.Models;
using AzR.AuditLog.DataAccess.AuditLog;
using System.Collections.Generic;

namespace AzR.AuditLog.Business.Services
{
    public interface ISampleService
    {
        SampleViewModel Get(int id);
        void Delete(int id);
        List<SampleViewModel> GetAll(bool showAll);
        bool Update(SampleViewModel model);
        void Create(SampleViewModel model);
        List<ChangeLog> GetAudit(int id);
    }
}