﻿using Akka.Actor;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDB.Client.Prism.Core;
using WebDB.Messages;

namespace WebDB.Client.Prism.Modules.AkkaDataService.Actors
{
    public class TickMessage { }
    public class DBClientActor : ReceiveActor
    {
        private Tuple<string, string> _lastMessage;
        private IUnityContainer _container;
        private ActorSelection _dbRootActor;
        private IEventAggregator _eventAggregator;
        private List<string> _entityTypes;
        public DBClientActor(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            _dbRootActor = Context.System.ActorSelection("akka.tcp://WebDB@localhost:8081/user/DBEntityRoot");
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<WebDB.Client.Prism.Core.SearchEvent>()
                .Subscribe(HandleSearchEvent);
            _eventAggregator.GetEvent<EntityUpdatedEvent>().Subscribe(SendEntityUpdateToSystem);

            Context.System.Scheduler.ScheduleTellRepeatedly(0, 100, Self, new TickMessage(), Self);

            Receive<TickMessage>(message => HandleTick());
            Receive<Tuple<string, string>>((message) => HandleDoSearch(message.Item1, message.Item2));
            Receive<AkkaSearchResults>(message => HandleSearchResult(message));
            Receive<AkkaGetEntityTypesResponse>(message => HandleEntityTypesResponse(message));
            Receive<GetEntityTypes>(message => GetEntityTypesFromSystem());
            Receive<NotifySubscribersOfEntityChange>(message => HandleNotifySubscribersOfEntityChanged(message));

            GetEntityTypesFromSystem();
            _self = Self;
        }

        private void HandleNotifySubscribersOfEntityChanged(NotifySubscribersOfEntityChange message)
        {
            _eventAggregator.GetEvent<NotifySubscribersOfEntityChangeEvent>().Publish(message);
        }

        protected override void PostStop()
        {
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            base.PreRestart(reason, message);
        }

        private Queue<object> _queue = new Queue<object>();

        private void SendEntityUpdateToSystem(UpdateEntityRequest message)
        {
            _queue.Enqueue(message);
        }

        private void GetEntityTypesFromSystem()
        {
            _dbRootActor.Tell(new GetEntityTypes());
            if (_entityTypes == null)
                Context.System.Scheduler.ScheduleTellOnce(1000, Self, new GetEntityTypes(), Self);
        }

        private void HandleEntityTypesResponse(AkkaGetEntityTypesResponse message)
        {
            _entityTypes = message.EntityTypes;
            _eventAggregator.GetEvent<GetEntityTypesEvent>().Publish(message);
        }

        private void HandleTick()
        {
            if (_lastMessage != null)
            {
                _dbRootActor.Tell(new GetAllRequest(_lastMessage.Item1, _lastMessage.Item2));
                _lastMessage = null;
                _dbRootActor.Tell(new GetEntityTypes());
            }
            while (_queue.Count > 0)
            {
                var message = _queue.Dequeue();
                _dbRootActor.Tell(message);
            }
        }

        private void HandleDoSearch(string message, string filter)
        {
            //Self.Tell(message);
            _dbRootActor.Tell(new GetAllRequest(message, filter));
            
        }

        IActorRef _self;

        private void HandleSearchEvent(Tuple<string, string> message)
        {
            _dbRootActor.Tell(message, _self);
            _lastMessage = message;
        }

        private void HandleSearchResult(AkkaSearchResults message)
        {
            //_eventAggregator = _container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SearchResultsEvent>().Publish(message);
            //throw new NotImplementedException();
        }
    }
}
