using System.Collections.Generic;
using System.Reflection;

namespace WebDB.Model
{
    public interface IModelObject
    {
        int Id { get; }
        string DesignerId { get; }
        bool IsChanged { get; set; }
        List<List<object>> Relationships { get; set; }
        List<PropertyInfo> GetNavigationProperties();
    }
}
