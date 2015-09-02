using Akka.Actor;
using System.Collections.Generic;
using WebDB.Messages;
using System;

namespace WebDB.Actors
{
    public class ClientViewActor : ReceiveActor
    {
        private EntityTypesActor _entityType;
        private List<IActorRef> _entities;

        private readonly IActorRef _clientActor;
        private readonly IActorRef _genericDBQueryActor;
        private IActorRef _entityChangeNotificationActor;

        public ClientViewActor(IActorRef clientActor)
        {
            _clientActor = clientActor;
            _genericDBQueryActor = Context.ActorOf<GenericDBQueryActor>("GenericDBQueryActor"); // This should be a shared or pooled resource
            

            Receive<GetAllRequest>(message =>
            {
                _entityChangeNotificationActor = Context.System.ActorSelection($"/user/EntityChangeNotificationRootActor/{message.EntityType}").ResolveOne(TimeSpan.FromSeconds(1)).Result;
                _genericDBQueryActor.Tell(message);
            });

            Receive<AkkaSearchResults>(message =>
            {
                SubscribeToChangesForResults(message);
                _clientActor.Tell(message);
            });

            Receive<NotifySubscribersOfEntityChange>(message =>
            {
                _clientActor.Tell(message);
            });
        }

        private void SubscribeToChangesForResults(AkkaSearchResults message)
        {
            foreach (var entity in message.Results)
            {
                long entityId = entity.GetId();
                _entityChangeNotificationActor.Tell(new SubscribeToEntityChanged(entityId));
            }
        }
    }
}
