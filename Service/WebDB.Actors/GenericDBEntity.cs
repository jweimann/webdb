using System;
using System.Linq;
using Akka.Actor;
using WebDB.Messages;
using WebDB.EntityFramework;
using System.Reflection;
using System.Data.Entity;
using System.Collections.Generic;
using WebDB.Model;
using System.Data.Entity.Infrastructure;
using RefactorThis.GraphDiff;
using System.Collections;

namespace WebDB.Actors
{
    public class GenericDBEntity : ReceiveActor
    {
        public Type EntityType { get; private set; }
        public long Id { get; private set; }
        private object _modelObject;
        private object _dbModelObject;
        private Stack<object> _undoStack = new Stack<object>();
        private readonly IActorRef _entityChangeNotificationActor;

        public GenericDBEntity(string entityType, long id)
        {
            _entityChangeNotificationActor = Context.System.ActorSelection($"/user/EntityChangeNotificationRootActor/{entityType}").ResolveOne(TimeSpan.FromSeconds(1)).Result;

            var entityAssembly = Assembly.GetAssembly(typeof(IModelObject)); // Removed DTOs - ModelObject = ef entity
            var allEntityTypes = entityAssembly.GetTypes().ToList();

            EntityType = allEntityTypes.FirstOrDefault(t => t.Name == entityType);
            Id = id;

            _modelObject = LoadEntityFromDb();

            Receive<UpdateEntityRequest>(message =>
            {
                _undoStack.Push(_modelObject);

                _modelObject = message.GetModelObject();
                SaveModelObject();
                Sender.Tell(_modelObject);
                _entityChangeNotificationActor.Tell(new NotifySubscribersOfEntityChange(this.Id));
            });

            Receive<UndoRequest>(message =>
            {
                if (_undoStack.Count > 0)
                {
                    _modelObject = _undoStack.Pop();
                    SaveModelObject();
                }
                Sender.Tell(_modelObject);
            });
        }

        private void SaveModelObject()
        {
            if (_dbModelObject == null)
                GetDBModelObject();

            using (WebDbPoliticsModel context = new WebDbPoliticsModel())
            {
                DbSet set = context.Set(EntityType);
                //var existing = set.Find(Id);
                

                var entry = context.Entry(_dbModelObject);
                set.Attach(_dbModelObject);
                entry.CurrentValues.SetValues(_modelObject);
                //entry.State = EntityState.Modified;

                var navigationProperties = (_modelObject as IModelObject).GetNavigationProperties();
                foreach (var property in navigationProperties)
                {
                    Type navigationType = property.PropertyType;
                    DbSet navSet = context.Set(navigationType.GenericTypeArguments.First());
                    var navigationItemCollection = property.GetValue(_modelObject) as IEnumerable;
                    foreach (var navigationItem in navigationItemCollection)
                    {
                        long id = navigationItem.GetId();
                        if (id > 0)
                        {
                            var existingNavigationItem = navSet.Find(id);
                            context.Entry(existingNavigationItem).CurrentValues.SetValues(navigationItem);
                        }
                        else
                        {
                            navSet.Add(navigationItem);
                        }
                    }
                    //navSet.Attach(navigationItem);
                }
                //context.UpdateGraph(_modelObject);

                // Add extension method for getting navigation properties of a modelobject
                // Then iterate them here and save any that are modified.

                //set.Add(_modelObject);
                //context.Entry(_modelObject).State = EntityState.Modified;
                var validationErrors = context.GetValidationErrors();
                //existing = _modelObject;
                context.SaveChanges();

                _dbModelObject = _modelObject;
            }
        }

      

        private void GetDBModelObject()
        {
            using (WebDbPoliticsModel context = new WebDbPoliticsModel())
            {
                DbSet set = context.Set(EntityType);

                //var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
                //return objectStateEntry.EntityKey.EntityKeyValues[0].Value;

                _dbModelObject = set.Find(_modelObject.GetId());
            }
        }

        private object LoadEntityFromDb()
        {
            using (WebDbPoliticsModel context = new WebDbPoliticsModel())
            {
                context.Configuration.ProxyCreationEnabled = false;
                DbSet set = context.Set(EntityType);
                return set.Find(Id);
            }
        }
    }
}
