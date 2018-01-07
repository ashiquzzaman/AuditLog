using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AzR.AuditLog.DataAccess.AuditLog
{
    public class EntityValue
    {
        public static object NewObject(DbEntityEntry entry)
        {
            var newEntity = entry.CurrentValues.PropertyNames.ToDictionary(key => key,
                key => entry.CurrentValues.GetValue<object>(key));
            var eType = entry.Entity.GetType();
            var type = Type.GetType(eType.FullName);
            var newObj = Activator.CreateInstance(type);
            foreach (var kv in newEntity)
            {
                if (type.GetProperty(kv.Key) != null)
                {
                    type.GetProperty(kv.Key).SetValue(newObj, kv.Value);
                }

            }
            return newObj;
        }
        public static object OldObject(DbEntityEntry entry)
        {
            var oldEntity = entry.OriginalValues.PropertyNames.ToDictionary(key => key,
                key => entry.OriginalValues.GetValue<object>(key));
            var eType = entry.Entity.GetType();
            var type = Type.GetType(eType.FullName);
            var oldObj = Activator.CreateInstance(type);
            foreach (var kv in oldEntity)
            {
                if (type.GetProperty(kv.Key) != null)
                {
                    type.GetProperty(kv.Key).SetValue(oldObj, kv.Value);
                }

            }
            return oldObj;
        }
    }

}
