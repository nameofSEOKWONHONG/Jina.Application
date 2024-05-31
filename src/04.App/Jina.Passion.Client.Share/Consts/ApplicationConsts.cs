namespace Jina.Passion.Client.Share.Consts;

public class ApplicationConsts
{
    public static class SignalR
        {
            public const string HubUrl = "/signalRHub";
            public const string SystemHubUrl = "/systemHub";
            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessageAsync";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";
            public const string SystemChangeAsync = "SystemChangeAsync";

            public const string PingRequest = "PingRequestAsync";
            public const string PingResponse = "PingResponseAsync";
        }

        public static class Cache
        {
            public const string GetAllBrandsCacheKey = "all-brands";
            public const string GetAllDocumentTypesCacheKey = "all-document-types";

            public static string GetAllEntityExtendedAttributesCacheKey(string entityFullName)
            {
                return $"all-{entityFullName}-extended-attributes";
            }

            public static string GetAllEntityExtendedAttributesByEntityIdCacheKey<TEntityId>(string entityFullName, TEntityId entityId)
            {
                return $"all-{entityFullName}-extended-attributes-{entityId}";
            }
        }

        public static class MimeTypes
        {
            public const string OpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string Excel = "application/octet-stream";
        }

        public static class Encryption
        {
            public const string DB_ENC_SHA512_KEY = "AKIEUDNXMZ8823@28S3!!";
            public const string DB_ENC_IV = "WXNDFLGSZFOQJKJK";
            public const string DB_ENC_KEY = "BLKQJFFFBJUQUBHIAICLMJFVZZNLTXII";
        }

        public class Client
        {
            public const string CULTURE_CODE_NAME = "BlazorCulture";
            public const string INDEXED_DB_NAME = "demodb";
            public const string INDEXED_DB_STORE_NAME = "data-store";

            public enum ComponentViewMode
            {
                View,
                Dialog
            }
        }
        
        /// <summary>
        /// 감사 관련 사용자 행위 타입
        /// </summary>
        public class BizAuditMethodType
        {
            public static string Search = nameof(Search);
            public static string Add = nameof(Add);
            public static string Update = nameof(Update);
            public static string Remove = nameof(Remove);
            public static string Restore = nameof(Restore);
            public static string Import = nameof(Import);
            public static string Export = nameof(Export);
        }
        
        public class ConvertValues
        {
            public static double PoundToKg = 0.453592;
        }
}