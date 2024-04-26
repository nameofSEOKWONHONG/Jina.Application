using eXtensionSharp;
using Jina.Domain.Shared.Consts;

namespace Jina.Domain.Service.Infra
{
	public static class EncryptionExtensions
    {
        public static string vToAESEncrypt(this string text)
        {
            if (text.xIsEmpty()) return string.Empty;
            return text.xToAESEncrypt(ApplicationConsts.Encryption.DB_ENC_KEY,
                ApplicationConsts.Encryption.DB_ENC_IV);
        }

        public static string vToAESDecrypt(this string text)
        {
            if (text.xIsEmpty()) return String.Empty;
            if (text.xContains("SYSTEM")) return text;

            var decText = string.Empty;
            try
            {
                decText = text.xToAESDecrypt(ApplicationConsts.Encryption.DB_ENC_KEY,
                    ApplicationConsts.Encryption.DB_ENC_IV);
            }
            catch
            {
                decText = text;
            }

            return decText;
        }
    }
}