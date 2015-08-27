using System.Collections.Generic;
using System.Reflection;

namespace WebDB.Model
{
    public interface IModelObject
    {
        bool IsChanged { get; set; }
        List<List<object>> Relationships { get; set; }
        List<PropertyInfo> GetNavigationProperties();
    }
}
