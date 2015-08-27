namespace WebDB.Model
{
    public partial class PollIssue : ModelObjectBase
    {
        public int Id { get; set; }

        public int PollId { get; set; }

        public int IssueId { get; set; }

        public int PositionId { get; set; }

        public int PartyId { get; set; }

        public decimal Percentage { get; set; }

        public virtual Issue Issue { get; set; }

        public virtual Party Party { get; set; }

        public virtual Poll Poll { get; set; }

        public virtual Position Position { get; set; }
    }
}
