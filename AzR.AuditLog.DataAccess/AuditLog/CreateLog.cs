using AzR.AuditLog.DataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AzR.AuditLog.DataAccess.AuditLog
{
    public class CreateLog
    {
        public static void Create<T>(ActionType action, int keyFieldId, T oldObject, T newObject)
        {
            // get the difference
            var deltaList = newObject.Compare(oldObject);
            var audit = new AuditLog
            {
                LoginId = 0,
                ActionType = action,
                EntityName = typeof(T).Name,
                ActionTime = DateTime.Now,
                KeyFieldId = keyFieldId,
                ActionBy = "",
                ActionUrl = "",
                ValueBefore = JsonConvert.SerializeObject(oldObject),
                ValueAfter = JsonConvert.SerializeObject(newObject),
                ValueChange = JsonConvert.SerializeObject(deltaList)
            };
            // if use xml instead of json, can use xml annotation to describe field names etc better
            var ent = new ApplicationDbContext();
            ent.AuditLogs.Add(audit);
            ent.SaveChanges();

        }
        public static AuditLog Create(DbEntityEntry entry)
        {

            ActionType operation;
            switch (entry.State)
            {
                case EntityState.Added:
                    operation = ActionType.Create;
                    break;
                case EntityState.Deleted:
                    operation = ActionType.Delete;
                    break;
                case EntityState.Modified:
                    operation = ActionType.Update;
                    break;
                case EntityState.Unchanged:
                    operation = ActionType.Create;
                    break;
                default:
                    operation = ActionType.Create;
                    break;
            }

            object oldObject;
            IEnumerable<ObjectChangeLog> deltaList;
            var newObject = EntityValue.NewObject(entry);
            var eType = entry.Entity.GetType();
            if (entry.State == EntityState.Added)
            {
                var type = Type.GetType(eType.FullName);
                oldObject = Activator.CreateInstance(type);
                deltaList = newObject.Compare();
            }
            else
            {
                oldObject = EntityValue.OldObject(entry);
                deltaList = newObject.Compare(oldObject);
            }

            var audit = new AuditLog
            {
                LoginId = 0,
                ActionType = operation,
                EntityName = eType.Name,
                ActionTime = DateTime.Now,
                KeyFieldId = entry.OriginalValues.GetValue<int>("Id"),
                ActionBy = "",
                ActionUrl = "",
                ValueBefore = JsonConvert.SerializeObject(oldObject),
                ValueAfter = JsonConvert.SerializeObject(newObject),
                ValueChange = JsonConvert.SerializeObject(deltaList)
            };
            return audit;
        }


    }
}
