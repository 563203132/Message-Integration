using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Message.Integration.Common;
using Message.Integration.Common.Configurations;
using Message.Integration.Common.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Message.Integration.Aliyun.Clients
{
    public class MessageClient : IMessageClient
    {
        public readonly IOptions<AliyunSMSConfig> _config;

        public MessageClient(IOptions<AliyunSMSConfig> config)
        {
            _config = config;
        }

        public void Send(string phoneNumber, string code)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _config.Value.AccessKeyId,  string.Empty);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = _config.Value.Domain;
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", phoneNumber);
            request.AddQueryParameters("SignName", _config.Value.SignName);
            request.AddQueryParameters("TemplateCode", _config.Value.TemplateCode);
            request.AddQueryParameters("TemplateParam", JsonConvert.SerializeObject(new MessageContent { Code = code }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            CommonResponse response = client.GetCommonResponse(request);
            Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));

            if (response.HttpStatus >= 400)
            {
                AliyunSMSReponse content = null;

                try
                {
                    content = JsonConvert.DeserializeObject<AliyunSMSReponse>(response.Data);
                }
                catch (Exception ex)
                {
                    throw new MessageException(Constants.CODE_UNKONWN_ERROR, response.Data, ex);
                }

                throw new MessageException(Constants.CODE_UNKONWN_ERROR, content.Message);
            }
        }
    }
}
