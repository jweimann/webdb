using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class EntityChangeNotificationActor : ReceiveActor
    {
        private string _entityType;
        private Dictionary<long, List<IActorRef>> _subscriptionsPerEntity;
        public EntityChangeNotificationActor(string entityType)
        {
            _entityType = entityType;
            _subscriptionsPerEntity = new Dictionary<long, List<IActorRef>>();

            Receive<SubscribeToEntityChanged>(message =>
            {
                if (_subscriptionsPerEntity.ContainsKey(message.EntityId) == false)
                    _subscriptionsPerEntity.Add(message.EntityId, new List<IActorRef>());

                _subscriptionsPerEntity[message.EntityId].Add(Sender);
            });

            Receive<NotifySubscribersOfEntityChange>(message =>
            {
                if (_subscriptionsPerEntity.ContainsKey(message.EntityId))
                {
                    foreach (var subscriber in _subscriptionsPerEntity[message.EntityId])
                        subscriber.Tell(message);
                }
            });
        }
    }
}
