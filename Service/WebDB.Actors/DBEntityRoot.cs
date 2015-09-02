using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebDB.Messages;
using WebDB.Model;

namespace WebDB.Actors
{
    public class DBEntityRoot : ReceiveActor
    {
        
        private readonly IActorRef _queryTypeSwitcher;
        private readonly IActorRef _entityChangeNotificationRootActor;

        public DBEntityRoot()
        {
            _queryTypeSwitcher = Context.ActorOf<QueryTypeSwitcher>("QueryTypeSwitcher");

            Props props = Props.Create<EntityChangeNotificationRootActor>();
            _entityChangeNotificationRootActor = Context.System.ActorOf<EntityChangeNotificationRootActor>("EntityChangeNotificationRootActor");

            string mypath = Self.Path.ToString();
            //Receive<GetAllRequest>(message => _queryTypeSwitcher.Tell(message, Sender));

            Receive<AddEntityRequest>(message => 
            {
                //var modifiedObject = GetObjectFromJObject(message.ModelObject, message.EntityType);
                //var newMessage = new AddEntityRequest(modifiedObject, message.EntityType);
                _queryTypeSwitcher.Tell(message, Sender);
            });

            ReceiveAny(message =>
            {
                _queryTypeSwitcher.Tell(message, Sender);
            });
        }


        private object GetObjectFromJObject(object jObject, string entityTypeName)
        {
            Newtonsoft.Json.Linq.JObject jsonObject = jObject as Newtonsoft.Json.Linq.JObject;

            var entityAssembly = Assembly.GetAssembly(typeof(EntityFramework.WebDbPoliticsModel));
            Type entityType = entityAssembly.GetTypes().FirstOrDefault(t => t.Name == entityTypeName);
            object modelObject = jsonObject.ToObject(entityType);
            return modelObject;
        }
    }
}
