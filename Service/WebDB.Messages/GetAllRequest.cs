namespace WebDB.Messages
{
    public class GetAllRequest
    {
        public GetAllRequest(string entityType)
        {
            EntityType = entityType;
        }

        public string EntityType { get; private set; }
    }
}
