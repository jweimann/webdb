using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebDB.EntityFramework;
using WebDB.Messages;
using WebDB.Model;
using System.Data.Entity;
using AutoMapper;

namespace WebDB.Actors
{
    public class GenericDBQueryActor : ReceiveActor
    {
        public GenericDBQueryActor()
        {
            Receive<GetAllRequest>(message =>
            {
                using (WebDbPoliticsModel model = new WebDbPoliticsModel())
                {
                    model.Configuration.ProxyCreationEnabled = false;
                    var entityAssembly = Assembly.GetAssembly(typeof(IModelObject)); // was WebDbPoliticsModel before removing dtos
                    var allEntityTypes = entityAssembly.GetTypes().ToList();
                    Type type = allEntityTypes.FirstOrDefault(t => t.Name == message.EntityType);

                    if (type == null)
                    {
                        Sender.Tell("ERROR");
                        return;
                    }


                    var set = model.Set(type);//.Include("PollIssues");

                    var relationshipProperties = type.GetProperties()
                  .Where(t =>
                          t.Name != "Relationships" &&
                          t.GetGetMethod().IsVirtual &&
                          t.PropertyType.IsGenericType &&
                          t.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                  .ToList();

                    var query = set.AsQueryable();

                    foreach (var relatedProperty in relationshipProperties)
                    {
                        query = query.Include(relatedProperty.Name);
                    }
                    ///set.AsQueryable().


                    var result = query.AsNoTracking().ToListAsync().Result;

                    //HandleDbResponse(result, type);
                    Sender.Tell(new AkkaSearchResults(message.EntityType, result));

                    //Sender.Tell(new AkkaSearchResults(message.EntityType, new List<object>() { "string" }));


                }
            });
        }

        private void HandleDbResponse(List<object> result, Type entityType)
        {
            var modelAssembly = Assembly.GetAssembly(typeof(IModelObject));

            var allModelTypes = modelAssembly.GetTypes().ToList();
            Type destinationType = allModelTypes.FirstOrDefault(t => t.Name == entityType.Name);

            string destTypeName = $"System.Collections.Generic.List`1[[{destinationType.FullName}, {modelAssembly.FullName}]";

            Type destList = Type.GetType(destTypeName);

            Mapper.CreateMap(entityType, destinationType);

            List<object> output = new List<object>();
            foreach (var item in result)
            {
                output.Add(Mapper.Map(item, item.GetType(), destinationType));
            }

            Sender.Tell(new AkkaSearchResults(entityType.Name, output));
        }
    }
}
