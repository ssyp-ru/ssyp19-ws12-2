using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("conversation/list")]
    [ApiController]
    public class ConversationListController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var request = JsonConvert.DeserializeObject<ConversationListRequest>(value);
        }
    }
}