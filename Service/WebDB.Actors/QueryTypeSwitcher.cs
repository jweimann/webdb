using Akka.Actor;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class QueryTypeSwitcher : ReceiveActor
    {
        private readonly IActorRef _genericSelector;
        private readonly IActorRef _genericAdder;
        private readonly IActorRef _genericUpdater;
        private readonly IActorRef _entityTypes;

        public QueryTypeSwitcher()
        {
            _genericSelector = Context.ActorOf<GenericSelector>("GenericSelector");
            _genericAdder = Context.ActorOf<GenericAdder>("GenericAdder");
            _genericUpdater = Context.ActorOf<GenericUpdater>("GenericUpdater");
            _entityTypes = Context.ActorOf<EntityTypes>("EntityTypes");

            Receive<GetAllRequest>(message =>
            {
                _genericSelector.Tell(message, Sender);
            });

            Receive<AddEntityRequest>(message =>
            {
                _genericAdder.Tell(message, Sender);
            });

            Receive<UpdateEntityRequest>(message =>
            {
                _genericUpdater.Tell(message, Sender);
            });

            Receive<UndoRequest>(message =>
            {
                _genericUpdater.Tell(message, Sender);
            });

            Receive<GetEntityTypes>(message =>
            {
                _entityTypes.Tell(message, Sender);
            });
        }
    }
}
