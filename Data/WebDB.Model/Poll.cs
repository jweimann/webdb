namespace WebDB.Model
{
    using System;
    using System.Collections.Generic;

    public partial class Poll : ModelObjectBase
    {
        public Poll()
        {
            PollIssues = new HashSet<PollIssue>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime PollDate { get; set; }

        public virtual ICollection<PollIssue> PollIssues { get; set; }
    }
}
