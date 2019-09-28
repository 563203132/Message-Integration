using Message.Integration.Aliyun.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Message.Integration.Api.Controllers
{
    [Route("api/v1/aliyun")]
    [ApiController]
    public class AliyunController : ControllerBase
    {
        private readonly IMessageClient _messageClient;
      
        public AliyunController(IMessageClient messageClient)
        {
            _messageClient = messageClient;
        }

        [HttpGet("send")]
        public ActionResult SendMessage([FromQuery]string accessKeyId, [FromQuery]string secret, [FromQuery]string message)
        {
            _messageClient.Send(accessKeyId, secret, message);

            return Ok();
        }
    }
}
