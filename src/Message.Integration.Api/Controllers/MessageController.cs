using Message.Integration.Aliyun.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Message.Integration.Api.Controllers
{
    [Route("api/v1/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageClient _messageClient;

        public MessageController(IMessageClient messageClient)
        {
            _messageClient = messageClient;
        }

        [HttpGet("push")]
        public ActionResult Push([FromQuery]string accessKeyId, [FromQuery]string secret, [FromQuery]string message)
        {
            var messageContent = new MessageContent
            {
                Code = message
            };

            _messageClient.Send(accessKeyId, secret, messageContent);

            return Ok();
        }
    }
}
