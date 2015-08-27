using Akka.Actor;
using System.Collections.Generic;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class GenericDBUpdaterActor : ReceiveActor
    {
        private Dictionary<long, IActorRef> _dbUpdateEntityActors = new Dictionary<long, IActorRef>();
        public GenericDBUpdaterActor()
        {
            Receive<UpdateEntityRequest>(message =>
            {
                object modelObject = message.GetModelObject();

                long id = message.GetId();
                if (_dbUpdateEntityActors.ContainsKey(id) == false)
                {
                    Props props = Props.Create<GenericDBEntity>(new object[] { message.EntityType, id });
                    string entityName = $"{message.EntityType}_{id}";
                    _dbUpdateEntityActors.Add(id, Context.ActorOf(props, entityName));
                }
                _dbUpdateEntityActors[id].Tell(message, Sender);
            });

            Receive<UndoRequest>(message =>
            {
                if (_dbUpdateEntityActors.ContainsKey(message.Id))
                    _dbUpdateEntityActors[message.Id].Tell(message, Sender);
                else
                    Sender.Tell("Not Found");
            });
        }
    }
}