using System.Collections.Generic;
using PropertyChanged;

namespace WebDB.Model
{
    [ImplementPropertyChanged]

    public class Party : ModelObjectBase
    {
        public Party()
        {
            PollIssues = new List<PollIssue>();
        }
        //public override  int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PollIssue> PollIssues { get; set; }
    }
}
