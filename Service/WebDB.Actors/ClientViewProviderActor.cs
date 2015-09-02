using Akka.Actor;
using WebDB.Messages;

namespace WebDB.Actors
{
    public class ClientViewProviderActor : ReceiveActor
    {
        private readonly IActorRef _genericSelector;

        public ClientViewProviderActor()
        {
            _genericSelector = Context.ActorOf<GenericSelector>("GenericSelector");

            Receive<GetAllRequest>(message =>
            {
                Props props = Props.Create<ClientViewActor>(new object[] { Sender });
                IActorRef clientView = Context.ActorOf(props);
                clientView.Tell(message);
                //_genericSelector.Tell()
                //message.EntityType
            });
        }
    }
}
