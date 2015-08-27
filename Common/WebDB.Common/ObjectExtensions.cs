using System;
using System.Reflection;

namespace WebDB.Common
{
    public static class ObjectExtensions
    {
        public static long GetId(this object resultObject)
        {
            Type type = resultObject.GetType();
            PropertyInfo property = type.GetProperty("Id");
            object value = property.GetValue(resultObject);

            long id;
            Int64.TryParse(value.ToString(), out id);

            return id;
        }
    }
}
