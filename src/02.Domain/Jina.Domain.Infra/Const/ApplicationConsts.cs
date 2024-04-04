using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Service.Infra.Const
{
    public class ApplicationConsts
    {
        public class Encryption
        {
            public const string DB_ENC_SHA512_KEY = "AKIEUDNXMZ8823@28S3!!";
            public const string DB_ENC_IV = "WXNDFLGSZFOQJKJK";
            public const string DB_ENC_KEY = "BLKQJFFFBJUQUBHIAICLMJFVZZNLTXII";
        }

        public class Redis
        {
            public const string MessageChannel = "EWS:REDIS:CHANNEL";
        }

        public class SignalR
        {
            public static string NotificationHubName = "notificationHub";
        }

        public class Limit
        {
            public const int ACCESS_LIMIT_COUNT = 5;
        }
    }
}