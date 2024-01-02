using eXtensionSharp;
using Jina.Domain.Infra.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Infra.Extension
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