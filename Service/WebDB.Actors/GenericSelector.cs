using Akka.Actor;
using System.Collections.Generic;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class GenericSelector : ReceiveActor
    {
        private Dictionary<string, IActorRef> _dbQueryActors = new Dictionary<string, IActorRef>();
        public GenericSelector()
        {
            Receive<GetAllRequest>(message =>
            {
                var typeName = message.EntityType;
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    Sender.Tell("ERROR NO TYPENAME SPECIFIED!");
                    return;
                }
                
                if (_dbQueryActors.ContainsKey(typeName) == false)
                {
                    _dbQueryActors.Add(typeName, Context.ActorOf<GenericDBQueryActor>(typeName));
                }
                var path = _dbQueryActors[typeName];
                path.Tell(message, Sender);
                //path.Tell(new GetAllRequest(typeName), Sender);
            });
        }
    }
}
