
using AzR.AuditLog.DataAccess.AuditLog;
using System;

namespace AzR.AuditLog.DataAccess.Entities
{
    public class Sample
    {
        [IgnoreLog]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Active { get; set; }
    }
}
