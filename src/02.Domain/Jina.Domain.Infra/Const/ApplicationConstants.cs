using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Infra.Const
{
    public class ApplicationConsts
    {
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