using a7D.PDV.BLL;
using a7D.PDV.Configurador.UI;
using a7D.PDV.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace a7D.PDV.AtualizarLicencas
{
    public static class Program
    {
        private const string api = "http://apipdvseven.azurewebsites.net/";
        //private const string api = "http://testeapipdvseven.azurewebsites.net/";
        static void Main(string[] args)
        {
            Console.Title = "Atualização de Licenças PDVSeven";

            Console.WriteLine("Atualização de Licenças PDVSeven");
            Console.WriteLine();
            Console.WriteLine("O processo de atualização de licenças irá começar agora");
            Console.WriteLine("Cetifique-se que o computador está conectado à internet antes de começar");
            Console.WriteLine("Cetifique-se que todos os programas da solução PDVSeven estejam fechados");
            Console.WriteLine();
            Console.WriteLine("Pressione <ENTER> para continuar...");

            Console.ReadLine();
            Console.Clear();

            //Console.WriteLine("Insira o endereço do WebService: (Pressione <ENTER> para o valor padrão)");
            //Console.Write("(" + api + "): ");

            //var endereco = Console.ReadLine();

            //if (string.IsNullOrWhiteSpace(endereco))
            var endereco = Environment.GetEnvironmentVariable("YY-URLAtivacao-XX", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrWhiteSpace(endereco))
                endereco = api;

            Console.Clear();

            Console.WriteLine("Verificando conexão com a internet...");
            try
            {
                VerificarConexaoInternet();
                Console.WriteLine("Conectado à internet!");
                Console.WriteLine("Verificando conexão com banco de dados...");
                VerificarConexaoBD();
                Console.WriteLine("Conexão com o Banco de dados OK!");
                Console.WriteLine("Atualizando licenças...");
                Atualizar();
                Console.WriteLine("Comunicando as alterações");
                EnviarDadosLicenca(endereco);
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                Console.WriteLine("Em caso de dúvida entre em contato com o suporte.");
                Console.WriteLine();
                Console.WriteLine("Pressione qualquer tecla para encerrar");
                Console.ReadKey();
                return;
            }
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();
            Console.WriteLine("Licenças atualizadas com sucesso!");
            ConfiguracaoBD.AlterarConfiguracaoSistema("atualizado211", "1");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para encerrar");
            Console.ReadKey();
        }

        private static void EnviarDadosLicenca(string endereco)
        {
            var chaveAtivacao = new ConfiguracoesAtivacao().ChaveAtivacao;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endereco);
                client.DefaultRequestHeaders.Add("x-auth-token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6InNlcnZlciIsImlzcyI6InNlbGYiLCJuYmYiOjE0NjM0MzIxODJ9.8Uw2JjVgPRxGOuuy9HPDwnubnuKNytuWvO_-ToQIPpw");

                var json = BLL.PDV.JsonEnvio(chaveAtivacao);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("/api/licencas/enviar", content).Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static void VerificarConexaoBD()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["connectionString"];
            if (connectionString == null)
            {
                var frm = new frmConfigurarConexaoDB();
                frm.ShowDialog();
                Environment.Exit(0);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV ADD Dados varchar(max) NULL";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number != 2705)
                            if (ex.Number == 1088)
                                throw new Exception("Erro de nivel de acesso de usuario\nCertifique-se que o usuário utilizado é \"dbOwner\" antes de continuar");
                            else
                                throw new Exception("Não foi possível conectar-se ao banco de dados\n" + ex.Message);
                    }
                    finally { conn.Close(); }
                }
            }
        }

        private static void VerificarConexaoInternet()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ativacao.pdvseven.com.br");

                var result = client.GetAsync("/WebService.asmx").Result;
                if (!result.IsSuccessStatusCode)
                {
                    var status = result.ReasonPhrase;
                    var message = result.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException(status + "\n" + message);
                }
            }
        }

        public static void Atualizar()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                try
                {
                    Console.WriteLine("Abrindo conexão com banco de dados...");
                    connection.Open();
                    DataTable database = connection.GetSchema("DataBases");

                    Console.WriteLine("Atualizando licenças...");
                    var command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV ADD Dados varchar(max) NULL";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex) { if (ex.Number != 2705) throw; }

                    SqlDataAdapter da;
                    var ds = new DataSet();

                    da = new SqlDataAdapter(@"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'tbPDV'", connection);
                    da.Fill(ds);

                    var rows = ds.Tables[0].Rows;
                    if (rows.Count == 3)
                    {
                        if (rows[0].Field<string>(0) == "IDPDV"
                           && rows[1].Field<string>(0) == "Nome"
                           && rows[2].Field<string>(0) == "Dados")
                        {
                            return;
                        }
                    }


                    ds.Dispose();
                    ds = new DataSet();

                    Console.WriteLine("Atualizando licenças...");
                    da = new SqlDataAdapter("select * from dbo.tbPDV", connection);
                    da.Fill(ds);

                    List<PDVInformation> pdvs = new List<PDVInformation>();

                    Console.WriteLine("Atualizando licenças...");
                    foreach (var dr in ds.Tables[0].AsEnumerable())
                    {
                        pdvs.Add(BLL.PDV.NovoPDVInformation(dr));
                    }

                    da.Dispose();
                    ds.Dispose();

                    Console.WriteLine("Atualizando licenças...");
                    foreach (var pdv in pdvs)
                    {
                        BLL.PDV.Salvar(pdv, false);
                    }

                    command = connection.CreateCommand();
                    command.CommandText = @"
IF(OBJECT_ID('FK_tbPDV_tbTipoPDV', 'F') IS NOT NULL)
        ALTER TABLE dbo.tbPDV
            DROP CONSTRAINT FK_tbPDV_tbTipoPDV";
                    command.ExecuteNonQuery();

                    Console.WriteLine("Atualizando licenças...");
                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV DROP COLUMN IDTipoPDV";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV DROP COLUMN ChaveHardware";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV DROP COLUMN UltimoAcesso";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV DROP COLUMN UltimaAlteracao";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV DROP COLUMN Ativo";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE dbo.tbPDV DROP COLUMN Versao";
                    command.ExecuteNonQuery();

                }
                catch
                {
                    throw;
                }
            }
        }

        class PdvAtivacao
        {
            public int IdPdvAtivacao { get; set; }

            public string ChaveAtivacao { get; set; }

            public int IdPdv { get; set; }

            public int IdTipoPdv { get; set; }

            public string Nome { get; set; }

            public string ChaveHardware { get; set; }

            public DateTime? DtUltimaAlteracao { get; set; }
            public string Versao { get; set; }
        }
    }
}
