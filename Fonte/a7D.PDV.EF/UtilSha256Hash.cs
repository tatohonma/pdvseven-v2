using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace a7D.PDV.EF
{
    public static class UtilSha256Hash
    {
        public static string GenerateSHA256String(IEnumerable<object> valores)
        {
            using (var sha256 = SHA256Managed.Create())
            {
                StringBuilder dados = new StringBuilder();
                foreach (var valor in valores)
                {
                    if (valor == null)
                        dados.Append("!");
                    else
                        dados.Append(valor.ToString() + "-");
                }
                var textofinal = dados.ToString();
                var bt = UTF32Encoding.UTF8.GetBytes(textofinal);
                var hash = sha256.ComputeHash(bt);
                return GetStringFromHash(hash);
            }
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                result.Append(hash[i].ToString("X2"));

            return result.ToString();
        }
    }
}
