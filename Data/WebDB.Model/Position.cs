namespace WebDB.Model
{
    using System.Collections.Generic;

    public partial class Position : ModelObjectBase
    {
        public Position()
        {
            PollIssues = new HashSet<PollIssue>();
        }

        public int Id { get; set; }

        public int IssueId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Issue Issue { get; set; }

        public virtual ICollection<PollIssue> PollIssues { get; set; }
    }
}
