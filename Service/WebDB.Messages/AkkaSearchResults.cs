using System.Collections.Generic;

namespace WebDB.Messages
{
    public class AkkaSearchResults
    {
        public AkkaSearchResults(string entityType, List<object> results)
        {
            EntityType = entityType;
            Results = results;
        }

        public string EntityType { get; private set; }
        public List<object> Results { get; private set; }
    }
}
