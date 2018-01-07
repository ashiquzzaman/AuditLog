using System;
using System.Collections.Generic;
using System.Linq;
using AzR.AuditLog.DataAccess.Entities;
using Newtonsoft.Json;

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

    }

    public static class ObjectExtention
    {
        public static IEnumerable<ObjectChangeLog> Compare<T>(this T newModel, T oldModel)
        {
            var properties = typeof(T).GetProperties();
            //var className = oldEntry.GetType().Name;

            return (from property in properties
                    let oldValue = property != null
                                   && !Attribute.IsDefined(property, typeof(IgnoreLogAttribute))
                                   && property.GetValue(oldModel) != null
                        ? property.GetValue(oldModel).ToString()
                        : null
                    let newValue = property != null
                                   && !Attribute.IsDefined(property, typeof(IgnoreLogAttribute))
                                   && property.GetValue(newModel) != null
                        ? property.GetValue(newModel).ToString()
                        : null
                    where oldValue != newValue
                    select new ObjectChangeLog
                    {
                        FieldName = property.Name,
                        ValueBefore = oldValue,
                        ValueAfter = newValue
                    }).ToList();
        }

    }
}