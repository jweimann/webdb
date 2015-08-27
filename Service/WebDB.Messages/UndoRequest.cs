namespace WebDB.Messages
{
    public class UndoRequest : IHaveEntityType
    {
        public string EntityType { get; private set; }

        public long Id { get; private set; }

        public UndoRequest(string entityType, long id)
        {
            EntityType = entityType;
            Id = id;
        }
    }
}