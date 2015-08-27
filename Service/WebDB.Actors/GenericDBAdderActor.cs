using Akka.Actor;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using WebDB.EntityFramework;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class GenericDBAdderActor : ReceiveActor
    {
        public GenericDBAdderActor()
        {
            Receive<AddEntityRequest>(message =>
                {
                    Type type = message.GetType();

                    object result = WriteEntityToDb(message);
                    // Send message to create an update entity somewhere (another handler & the dbentity object)
                    // this may not be needed though if I only create the dbEntity on updates, need to decide if that's how it will work.
                    // may just need to send entity back to the user (should definitely do that)
                    // should also send back a status update to the user that it's updating before save completes and any failures.
                    Sender.Tell(result);
                });
        }

        private object WriteEntityToDb(AddEntityRequest message)
        {
            Newtonsoft.Json.Linq.JObject jsonObject = message.ModelObject as Newtonsoft.Json.Linq.JObject;

            var entityAssembly = Assembly.GetAssembly(typeof(EntityFramework.WebDbPoliticsModel));
            Type entityType = entityAssembly.GetTypes().FirstOrDefault(t => t.Name == message.EntityType);
            object modelObject = jsonObject.ToObject(entityType);

            var modelType = modelObject.GetType();

            using (WebDbPoliticsModel db = new WebDbPoliticsModel())
            {
                db.Configuration.ProxyCreationEnabled = false;
                DbSet set = db.Set(modelType);
                var resultObject = set.Add(modelObject);
                db.SaveChanges();
                return resultObject;
            }
        }

     
    }
}