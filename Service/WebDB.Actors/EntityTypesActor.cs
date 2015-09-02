using Akka.Actor;
using System.Collections.Generic;
using WebDB.Messages;
using System.Linq;
using System.Reflection;

namespace WebDB.Actors
{
    public class EntityTypesActor : ReceiveActor
    {
        private List<string> _entityTypes;
        public EntityTypesActor()
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
            var entityTypes = assembly.GetTypes()
                .ToList()
                .Where(t=> t.IsClass == true && t.IsAbstract == false && t.Name != "<>c")
                .ToList();

            _entityTypes = entityTypes
                .Select(t => t.Name)
                .ToList();
        }
    }
}