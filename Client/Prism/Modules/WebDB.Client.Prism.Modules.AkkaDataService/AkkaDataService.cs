using Akka.Actor;
using Akka.Configuration;
using Akka.DI.Core;
using Akka.DI.Unity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDB.Client.Prism.Modules.AkkaDataService.Actors;
using WebDB.Messages;

namespace WebDB.Client.Prism.Modules.AkkaDataService
{
    public class AkkaDataService : IAkkaDataService
    {
        private ActorSystem _system;
        private IDependencyResolver _resolver;
        // rename container ...
        public AkkaDataService(IUnityContainer container, IEventAggregator eventAggregator)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
		    applied-adapters = []
		    transport-protocol = tcp
		    port = 0
		    hostname = localhost
        }
    }
}
");

            _system = ActorSystem.Create("WebDB", config);
            _resolver = new UnityDependencyResolver(container, _system);

            var chatClient = _system.ActorOf(_resolver.Create<DBClientActor>(), "DBClientActor");
            _system.ActorSelection("akka.tcp://WebDB@localhost:8081/user/DBEntityRoot");
            chatClient.Tell(new GetAllRequest("Party"));
            /*
                while (true) { System.Threading.Thread.Sleep(10); }
                 
                    while (true)
                    {
                        var input = Console.ReadLine();
                        if (input.StartsWith("/"))
                        {
                            var parts = input.Split(' ');
                            var cmd = parts[0].ToLowerInvariant();
                            var rest = string.Join(" ", parts.Skip(1));

                            //if (cmd == "/nick")
                            //{
                            //    chatClient.Tell(new NickRequest
                            //    {
                            //        NewUsername = rest
                            //    });
                            //}
                        }
                        //else
                        //{
                        //    chatClient.Tell(new SayRequest()
                        //    {
                        //        Text = input,
                        //    });
                        //}
                    }
                }
                */
        }
    }
}
