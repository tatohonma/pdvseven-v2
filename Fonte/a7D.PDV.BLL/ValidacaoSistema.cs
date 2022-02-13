using a7D.PDV.EF.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;

namespace a7D.PDV.BLL
{
    public class ValidacaoSistema
    {
        public static String RetornarChaveConfig(String chaveSistema)
        {
            return GetHashString(chaveSistema).Substring(0, 12);
        }

        public static String RetornarSerialHD()
        {
            String drive = "c";
            ManagementObject disk;

            disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }


        public static bool VerificarConexao()
        {
            try
            {
                using (var conn = new SqlConnection(DAL.DB.ConnectionString))
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool VerificarConexaoEVersaoDB()
        {
            try
            {
                using (var cn = new SqlConnection(DAL.DB.ConnectionString))
                {
                    cn.Open();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E103, ex);
                return false;
            }

            try
            {
                using (var cn = new pdv7Context())
                {
                    var versao = cn.Versoes.OrderByDescending(v => v.IDVersao).FirstOrDefault();
                    if (versao == null)
                    {
                        Logs.ErroBox(CodigoErro.A6F6);
                        return false;
                    }

                    var asmAPP = Assembly.GetCallingAssembly().GetName().Version;
                    var asmBLL = typeof(ValidacaoSistema).Assembly.GetName().Version;
                    var admDAL = typeof(DAL.DB).Assembly.GetName().Version;
                    var admEF = typeof(EF.Repositorio).Assembly.GetName().Version;

                    if (asmAPP != asmBLL || asmAPP != admDAL || asmAPP != admEF)
                    {
                        Logs.ErroBox(CodigoErro.A6F7);
                        return false;
                    }

                    var asmDB = new Version(versao.Versao);
                    if (asmDB != asmAPP)
                    {
                        if (asmDB.Major != asmAPP.Major
                         || asmDB.Minor != asmAPP.Minor
                         || asmDB.Build != asmAPP.Build) // || asmDB.Revision != asmAPP.Revision)
                        {
                            Logs.ErroBox(CodigoErro.A6F8, info: $"Banco de Dados: {asmDB}\r\nAplicação: {asmAPP}");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.A6F5, ex);
                return false;
            }
        }

        public static String GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Encoding.UTF8.GetBytes(inputString))
                sb.Append(Convert.ToInt32(Convert.ToInt32(b) * 0.33));

            return sb.ToString();
        }
    }
}
