using System.Collections.Generic;

namespace WebDB.Messages
{
    public class AkkaGetEntityTypesResponse
    {
        public List<string> EntityTypes { get; private set; }
        public AkkaGetEntityTypesResponse(List<string> entityTypes)
        {
            EntityTypes = entityTypes;
        }
    }
}
