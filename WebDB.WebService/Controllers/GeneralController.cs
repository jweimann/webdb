using System;
using System.Web.Http;
using Akka.Actor;
using System.Threading.Tasks;
using WebDB.Messages;

namespace WebDB.WebService.Controllers
{
    public class GeneralController : ApiController
    {
        private ActorSelection _dbEntityRoot;
        public GeneralController()
        {
            _dbEntityRoot = WebApiApplication.System.ActorSelection("akka://WebDB/user/DBEntityRoot");
        }

        [HttpGet]
        [Route("api/General/EntityTypes")]
        public async Task<object> GetEntityTypes()
        {
            var result = await _dbEntityRoot.Ask<object>(new GetEntityTypes());
            return result;
        }

        [HttpGet]
        public async Task<object> Get(string entityType)
        {
            //var resolved = await _dbEntityRoot.ResolveOne(TimeSpan.FromSeconds(1));
            var result = await _dbEntityRoot.Ask<object>(new GetAllRequest(entityType));
            return result;
        }

        [HttpPost]
        public async Task<object> Post(object modelObject, string entityType)
        {
            var result = await _dbEntityRoot.Ask<object>(new AddEntityRequest(modelObject, entityType));
            return result;
        }

        [HttpPut]
        public async Task<object> Put(object modelObject, string entityType)
        {
            var result = await _dbEntityRoot.Ask<object>(new UpdateEntityRequest(modelObject, entityType));
            return result;
        }

        [HttpPatch]
        public async Task<object> Undo(string entityType, long id)
        {
            var result = await _dbEntityRoot.Ask<object>(new UndoRequest(entityType, id));
            return result;
        }

        public async Task<object> Get(string entityType, long id)
        {
            return null;
        }
    }
}
