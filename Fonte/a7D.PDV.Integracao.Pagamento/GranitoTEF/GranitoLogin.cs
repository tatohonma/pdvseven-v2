using System;
using System.Security.Cryptography;
using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public static class GranitoLogin
    {
        internal static string CNPJ; // "22177858000169"
        internal static string Senha; // "1234567240"
        internal static string PDV; // "001"

        public static bool Is(string cnpj, string senha)
        {
            return CNPJ == cnpj && Senha == senha;
        }

        public static string Cript(string chave, string cnpj, string senha, int pdv)
        {
            using (var TDESAlgorithm = ObterAlgoritimo(chave))
            {
                var data = UTF8Encoding.UTF8.GetBytes($"{cnpj}|{senha}|{pdv.ToString("000")}");
                var encryptor = TDESAlgorithm.CreateEncryptor();
                var result = encryptor.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(result);
            }
        }

        public static void Decript(string chave, string valor)
        {
            using (var TDESAlgorithm = ObterAlgoritimo(chave))
            {
                var dataCript = Convert.FromBase64String(valor);
                var decryptor = TDESAlgorithm.CreateDecryptor();
                var result = decryptor.TransformFinalBlock(dataCript, 0, dataCript.Length);
                var data = UTF8Encoding.UTF8.GetString(result).Split('|');
                CNPJ = data[0];
                Senha = data[1];
                PDV = data[2];
            }
        }

        private static TripleDESCryptoServiceProvider ObterAlgoritimo(string chave)
        {
            using (var HashProvider = new MD5CryptoServiceProvider())
            {
                return new TripleDESCryptoServiceProvider()
                {
                    Key = HashProvider.ComputeHash(UTF8Encoding.UTF8.GetBytes(chave)),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                };
            }
        }
    }
}