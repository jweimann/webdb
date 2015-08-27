namespace WebDB.Model
{
    using System;
    using System.Collections.Generic;

    public partial class Issue : ModelObjectBase
    {
        public Issue()
        {
            PollIssues = new List<PollIssue>();
            Positions = new List<Position>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public virtual ICollection<PollIssue> PollIssues { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}
