using Akka.Actor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class GenericUpdater : ReceiveActor
    {
        private Dictionary<string, IActorRef> _dbQueryActors = new Dictionary<string, IActorRef>();

        public GenericUpdater()
        {
            Receive<IHaveEntityType>(message =>
            {
                string typeName = message.EntityType;
                if (_dbQueryActors.ContainsKey(typeName) == false)
                {
                    _dbQueryActors.Add(typeName, Context.ActorOf<GenericDBUpdaterActor>(typeName));
                }
                var path = _dbQueryActors[typeName];
                path.Tell(message, Sender);
            });
        }

        //long id = GetIDFromObject(message.ModelObject);
     
    }
}