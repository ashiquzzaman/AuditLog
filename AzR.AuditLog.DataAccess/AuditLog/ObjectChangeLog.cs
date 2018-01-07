namespace AzR.AuditLog.DataAccess.AuditLog
{
    public class ObjectChangeLog
    {
        public string FieldName { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
    }
}