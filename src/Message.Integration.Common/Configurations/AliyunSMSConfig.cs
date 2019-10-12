using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Integration.Common.Configurations
{
    public class AliyunSMSConfig
    {
        public string Domain { get; set; }

        public string SignName { get; set; }

        public string TemplateCode { get; set; }

        public string AccessKeyId { get; set; }

        public string AccessSecret { get; set; }
    }
}
