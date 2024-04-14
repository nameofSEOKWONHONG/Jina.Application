namespace Jina.Domain.SharedKernel.Consts
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

        public class Client
        {
            public const string CULTURE_NAME = "culture";
            public const string INDEXED_DB_NAME = "jina";
            public const string INDEXED_DB_STORE_NAME = "jina-data-store";
            public const string TITLE = "GlobalizeHub";
        }

        public class Timezone
		{
			public static Dictionary<string, string> Map = new Dictionary<string, string>()
		    {
			    { "ko-KR", "Korea Standard Time"},
			    { "en-US", "Eastern Standard Time"},
			    { "ja-JP", "Japan Standard Time"},
			    { "zh-CN", "China Standard Time"},
			    { "vi-VN", "Indochina Time"},
			    { "th-TH", "Indochina Time"},
		    };
		}
	}
}