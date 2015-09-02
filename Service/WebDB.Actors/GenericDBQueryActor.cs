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

                    var method = typeof(WebDbPoliticsModel).GetMethod("Set", new Type[0]).MakeGenericMethod(type);
                    var set = method.Invoke(model, new object[0]);
                    DbSet<Position> dbSet = set as DbSet<Position>;
                    
                    //DbSet<IModelObject> set = model.Set(type).Cast<IModelObject>();//.Include("PollIssues");

                    var relationshipCollectionProperties = type.GetProperties()
                  .Where(t =>
                          t.Name != "Relationships" &&
                          t.GetGetMethod().IsVirtual &&
                          t.PropertyType.IsGenericType &&
                          t.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                  .ToList();

                    var query = dbSet.AsQueryable();

                    foreach (var relatedProperty in relationshipCollectionProperties)
                    {
                        query = query.Include(relatedProperty.Name);
                    }
                    ///set.AsQueryable().

                    var relationshipSingleProperties = type.GetProperties()
                                            .Where(t => t.GetGetMethod().IsVirtual && 
                                            t.PropertyType.IsGenericType == false &&
                                            t.DeclaringType != typeof(ModelObjectBase))
                                            .ToList();

                    foreach(var relatedProperty in relationshipSingleProperties)
                    {
                        query = query.Include(relatedProperty.Name);
                    }
                    
                    if (!String.IsNullOrWhiteSpace(message.Filter))
                    {
                        query = query.Where(t => t.DesignerId.Contains(message.Filter));
                    }

                    var result = query.AsNoTracking().ToListAsync().Result;//.ConvertAll<object>();
                    List<object> resultsAsObjectList = new List<object>();
                    foreach (var r in result)
                        result.Add(r);
                    
                    Sender.Tell(new AkkaSearchResults(message.EntityType, resultsAsObjectList));

                    

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
