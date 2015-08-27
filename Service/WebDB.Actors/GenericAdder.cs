using Akka.Actor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class GenericAdder : ReceiveActor
    {
        private Dictionary<string, IActorRef> _dbQueryActors = new Dictionary<string, IActorRef>();
        public GenericAdder()
        {
            Receive<AddEntityRequest>(message =>
            {
                string typeName = message.EntityType;
                if (_dbQueryActors.ContainsKey(typeName) == false)
                {
                    _dbQueryActors.Add(typeName, Context.ActorOf<GenericDBAdderActor>(typeName));
                }
                var path = _dbQueryActors[typeName];
                path.Tell(message, Sender);
            });
        }
    }
}