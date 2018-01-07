using System.Collections.Generic;

namespace AzR.AuditLog.DataAccess.AuditLog
{
    public class ChangeLog
    {
        public ChangeLog()
        {
            Changes = new List<ObjectChangeLog>();
        }
        public string ActionTime { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionTypeName { get; set; }
        public string ActionBy { get; set; }
        public string ActionUrl { get; set; }
        public List<ObjectChangeLog> Changes { get; set; }


    }
}