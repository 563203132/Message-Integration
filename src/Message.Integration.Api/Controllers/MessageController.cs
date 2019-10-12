using CacheManager.Core;
using Message.Integration.Aliyun.Clients;
using Message.Integration.Api.Models;
using Message.Integration.Common;
using Message.Integration.Common.Exceptions;
using Message.Integration.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Message.Integration.Api.Controllers
{
    [Route("api/v1/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly string _region = "verification_code";

        private readonly IMessageClient _messageClient;
        private readonly ICacheManager<string> _cacheManager;

        public MessageController(
            IMessageClient messageClient,
            ICacheManager<string> cacheManager)
        {
            _messageClient = messageClient;
            _cacheManager = cacheManager;
        }

        [HttpGet("push")]
        public ActionResult Push([FromQuery]string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new MessageException(Constants.CODE_INVALID_PHONE_NUMBER, Constants.MESSAGE_INVALID_PHONE_NUMBER);
            }

            var code = string.Empty;

            do
            {
                code = RandomCodeHelper.Generate();
            }
            while (_cacheManager.Exists(GetCacheKey(phoneNumber, code), _region));

            _messageClient.Send(phoneNumber, code);

            _cacheManager.Add(new CacheItem<string>(GetCacheKey(phoneNumber, code), _region, string.Empty, ExpirationMode.Absolute, TimeSpan.FromMinutes(10)));

            return Ok(code);
        }

        [HttpPost("verify")]
        public ActionResult Verify([FromBody]MessageInfo message)
        {
            var cacheKey = GetCacheKey(message.PhoneNumber, message.Code);

            var isExists = _cacheManager.Exists(cacheKey, _region);

            return Ok(isExists);
        }

        private string GetCacheKey(string phoneNumber, string code)
        {
            return $"{phoneNumber}_{code}";
        }
    }
}
