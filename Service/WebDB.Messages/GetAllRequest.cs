namespace WebDB.Messages
{
    public class GetAllRequest
    {
        public GetAllRequest(string entityType, string filter = null)
        {
            EntityType = entityType;
            Filter = filter;
        }

        public string EntityType { get; private set; }
        public string Filter { get; private set; }
    }
}
