using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AzR.AuditLog.DataAccess.AuditLog
{
    public class CreateLog
    {
        public static AuditLog Create<T>(ActionType action, string keyFieldId, T oldObject, T newObject)
        {
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
            return audit;
        }
        public static AuditLog Create(DbEntityEntry entry, int status)
        {

            var actionType = (ActionType)status;
            object oldObject, newObject;
            string keyValue;
            IEnumerable<ObjectChangeLog> deltaList;
            var eType = entry.Entity.GetType();
            var type = Type.GetType(eType.FullName);

            switch (actionType)
            {
                case ActionType.Create:
                    {

                        oldObject = Activator.CreateInstance(type);
                        newObject = EntityValue.NewObject(entry);
                        deltaList = newObject.ToChangeLog();
                        keyValue = entry.CurrentValues.GetValue<object>("Id").ToString();
                        break;
                    }
                case ActionType.Delete:
                    oldObject = EntityValue.OldObject(entry);
                    newObject = oldObject;
                    deltaList = newObject.Compare(oldObject);
                    keyValue = entry.OriginalValues.GetValue<object>("Id").ToString();
                    break;
                default:
                    {
                        if (entry.OriginalValues.PropertyNames.Any(s => s == "Active") &&
                            !entry.CurrentValues.GetValue<bool>("Active"))
                        {
                            actionType = ActionType.Remove;
                        }
                        newObject = EntityValue.NewObject(entry);
                        oldObject = EntityValue.OldObject(entry);
                        deltaList = newObject.Compare(oldObject);
                        keyValue = entry.OriginalValues.GetValue<object>("Id").ToString();
                        break;
                    }
            }

            var audit = new AuditLog
            {
                LoginId = 0,
                ActionType = actionType,
                EntityName = eType.Name,
                ActionTime = DateTime.Now,
                KeyFieldId = keyValue,
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
