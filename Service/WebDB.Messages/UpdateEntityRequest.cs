namespace WebDB.Messages
{
    public class UpdateEntityRequest : IEntityRequest
    {
        public UpdateEntityRequest(object modelObject, string entityType)
        {
            ModelObject = modelObject;
            EntityType = entityType;
        }

        public string EntityType { get; private set; }
        public object ModelObject { get; private set; }
    }
}
