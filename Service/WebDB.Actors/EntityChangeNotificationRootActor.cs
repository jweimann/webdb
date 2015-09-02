using Akka.Actor;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class EntityChangeNotificationRootActor : ReceiveActor
    {
        public EntityChangeNotificationRootActor()
        {
            var entityTypeNames = Context.ActorOf<EntityTypesActor>().Ask<AkkaGetEntityTypesResponse>(new GetEntityTypes()).Result.EntityTypes;

            foreach (var entityTypeName in entityTypeNames)
            {
                Props props = Props.Create<EntityChangeNotificationActor>(entityTypeName);
                IActorRef child = Context.ActorOf(props, entityTypeName);
            }
        }
        
    }
}
