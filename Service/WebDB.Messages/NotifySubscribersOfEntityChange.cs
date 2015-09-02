namespace WebDB.Messages
{
    public class NotifySubscribersOfEntityChange
    {
        public long EntityId { get; private set; }
        public NotifySubscribersOfEntityChange(long entityId)
        {
            EntityId = entityId;
        }
    }
}
