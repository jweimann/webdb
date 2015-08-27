using System;
using System.Linq;
using System.Reflection;

namespace WebDB.Messages
{
    public static class EntityRequestExtensions
    {
        public static object GetModelObject(this IEntityRequest request)
        {
            if (request.ModelObject is Newtonsoft.Json.Linq.JObject == false)
            {
                return request.ModelObject; // Not using DTOs anymore..
                //var entityAssembly = Assembly.GetAssembly(typeof(EntityFramework.WebDbPoliticsModel));
                //Type entityType = entityAssembly.GetTypes().FirstOrDefault(t => t.Name == request.EntityType);
                //return AutoMapper.Mapper.DynamicMap(request.ModelObject, request.ModelObject.GetType(), entityType);
            }
            else
            {
                Newtonsoft.Json.Linq.JObject jsonObject = request.ModelObject as Newtonsoft.Json.Linq.JObject;

                var entityAssembly = Assembly.GetAssembly(typeof(EntityFramework.WebDbPoliticsModel));
                Type entityType = entityAssembly.GetTypes().FirstOrDefault(t => t.Name == request.EntityType);
                object modelObject = jsonObject.ToObject(entityType);
                return modelObject;
            }
        }

        public static long GetId(this IEntityRequest request)
        {
            return request.GetModelObject().GetId();
        }
        
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
