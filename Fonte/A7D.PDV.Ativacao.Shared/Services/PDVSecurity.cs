using System;
using System.Security.Cryptography;
using System.Text;

namespace a7D.PDV.Ativacao.Shared.Services
{
    public static class PDVSecurity
    {
        public const string senha = "ffaa1234";
        public const string dateformat = "yyyyMMddHHmmssfff";

        public static string GerarAtivacaoOffline(string chaveAtivacao)
        {
            var result = chaveAtivacao + DateTime.Now.ToUniversalTime().Date.ToString(dateformat);
            result = CalculateMD5Hash(result);
            return result.Length > 8 ? result.Substring(0, 8) : result;
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                var sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
