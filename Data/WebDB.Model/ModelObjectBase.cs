using PropertyChanged;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace WebDB.Model
{
    [ImplementPropertyChanged]
    public abstract class ModelObjectBase : IModelObject, INotifyPropertyChanged
    {
        public virtual int Id { get; private set; }
        public virtual string DesignerId { get; private set; }

        [NotMapped]
        public bool IsChanged { get; set; } = false;

        [NotMapped]
        public List<List<object>> Relationships { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<PropertyInfo> GetNavigationProperties()
        {
            Type type = this.GetType();
            var navigationProperties = type.GetProperties()
                  .Where(t =>
                          t.Name != "Relationships" &&
                          t.GetGetMethod().IsVirtual &&
                          t.PropertyType.IsGenericType &&
                          t.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                  .ToList();
            return navigationProperties;
        }
    }
}
