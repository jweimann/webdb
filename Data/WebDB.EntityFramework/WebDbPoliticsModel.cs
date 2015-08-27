namespace WebDB.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Model;

    public partial class WebDbPoliticsModel : DbContext
    {
        public WebDbPoliticsModel()
            : base("name=WebDBPoliticsModel")
        {
        }

        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<PollIssue> PollIssues { get; set; }
        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .HasMany(e => e.PollIssues)
                .WithRequired(e => e.Issue)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>()
                .HasMany(e => e.Positions)
                .WithRequired(e => e.Issue)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Party>()
                .HasMany(e => e.PollIssues)
                .WithRequired(e => e.Party)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PollIssue>()
                .Property(e => e.Percentage)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Poll>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Poll>()
                .HasMany(e => e.PollIssues)
                .WithRequired(e => e.Poll)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Position>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.PollIssues)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);
        }
    }
}
