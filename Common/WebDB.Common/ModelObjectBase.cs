using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDB.Common
{
    public abstract class EFModelObjectBase : IEFModelObject, INotifyPropertyChanged
    {
        [NotMapped]
        public bool IsChanged { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
