using Akka.Actor;
using System.Collections.Generic;
using WebDB.Messages;
using System.Linq;
using System.Reflection;

namespace WebDB.Actors
{
    internal class EntityTypes : ReceiveActor
    {
        private List<string> _entityTypes;
        public EntityTypes()
        {
            FillEntityTypes();

            Receive<GetEntityTypes>(message =>
            {
                AkkaGetEntityTypesResponse response = new AkkaGetEntityTypesResponse(_entityTypes);
                Sender.Tell(response);
            });
        }

        private void FillEntityTypes()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(WebDB.Model.IModelObject));
            _entityTypes = assembly.GetTypes()
                .ToList()
                .Where(t=> t.IsClass == true && t.IsAbstract == false)
                .Select(t => t.Name)
                .ToList();
        }
    }
}