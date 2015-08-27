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
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PollIssue> PollIssues { get; set; }
    }
}
