using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AzR.AuditLog.DataAccess.AuditLog
{
    public static class ObjectExtention
    {
        public static IEnumerable<ObjectChangeLog> Compare(this object newModel, object oldModel)
        {
            var properties = oldModel.GetType().GetProperties();
            return (from property in properties
                    let oldValue = property != null
                                   && !Attribute.IsDefined((MemberInfo)property, typeof(IgnoreLogAttribute))
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

        public static IEnumerable<ObjectChangeLog> Compare(this object newModel)
        {
            var properties = newModel.GetType().GetProperties();
            return (from property in properties
                    let newValue = property != null
                                   && !Attribute.IsDefined(property, typeof(IgnoreLogAttribute))
                                   && property.GetValue(newModel) != null
                        ? property.GetValue(newModel).ToString()
                        : null
                    select new ObjectChangeLog
                    {
                        FieldName = property.Name,
                        ValueBefore = null,
                        ValueAfter = newValue
                    }).ToList();
        }

    }
}