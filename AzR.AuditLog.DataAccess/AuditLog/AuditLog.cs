using System;
using System.ComponentModel.DataAnnotations;

namespace AzR.AuditLog.DataAccess.AuditLog
{
    public class AuditLog
    {
        public long Id { get; set; }
        public long LoginId { get; set; }
        [StringLength(128)]
        public string KeyFieldId { get; set; }
        public DateTime ActionTime { get; set; }
        [StringLength(256)]
        public string EntityName { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
        public string ValueChange { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionBy { get; set; }
        public string ActionUrl { get; set; }
    }
}
