namespace WebDB.Messages
{
    public class SubscribeToEntityChanged
    {
        public long EntityId { get; private set; }
        public SubscribeToEntityChanged(long entityId)
        {
            EntityId = entityId;
        }
    }
}
