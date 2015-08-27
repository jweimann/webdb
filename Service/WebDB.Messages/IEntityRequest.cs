namespace WebDB.Messages
{
    public interface IEntityRequest : IHaveEntityType
    {
        object ModelObject { get; }
    }
}
