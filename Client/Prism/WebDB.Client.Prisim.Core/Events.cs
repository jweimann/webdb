using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.Generic;
using WebDB.Messages;

namespace WebDB.Client.Prism.Core
{
    public class EntityUpdatedEvent : PubSubEvent<UpdateEntityRequest> { }
    public class SearchEvent : PubSubEvent<string> { }
    public class SearchResultsEvent : PubSubEvent<AkkaSearchResults> { }
    public class GetEntityTypesEvent : PubSubEvent<AkkaGetEntityTypesResponse> { }
}
