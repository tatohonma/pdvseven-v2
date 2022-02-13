using MuxxLib;
using MuxxLib.Enum_s;
using MuxxLib.Estruturas;
using MuxxLib.Interface;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{
    public class PGWebLib
    {
        private string PayGoPath;
        public PW_GetData[] GetData;
        private FactoryBase factoryOperation;

        public PWRET LastResult { get; private set; }
        public PWRET Result { get; private set; }
        public short ResultCode { get; private set; }
        public Exception Erro { get; internal set; }
        public short NumParam { get; private set; }

        private static bool dllInit = false;
        public PGWebLib(FactoryBase facOperation)
        {
            factoryOperation = facOperation;

            var fi = new FileInfo(Constantes.PGWebLibAdress);
            var di = new DirectoryInfo(Path.Combine(fi.Directory.FullName, "TEF"));
            if (!di.Exists)
                di.Create();

            PayGoPath = fi.Directory.FullName;
            GetData = new PW_GetData[Constantes.NUMPARAM];

            if (!dllInit)
            {
                try
                {
                    if (!fi.Exists)
                    {
                        if (!fi.Directory.Exists)
                            fi.Directory.Create();

                        var localPath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;

                        var local = new FileInfo(Path.Combine(localPath, fi.Name));
                        local.CopyTo(fi.FullName);

                        string certificado = "certificado.crt";
                         local = new FileInfo(Path.Combine(localPath, certificado));
                        local.CopyTo(Path.Combine(PayGoPath, certificado));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi posivel isntalar as DLL NTK\r\n" + ex.Message, ex);
                }

                if (Exec(() => Interop.PW_iInit(fi.Directory.FullName)))
                    dllInit = true;
            }
        }

        public void ResetDLL()
        {
            dllInit = false;
        }

        internal bool Display(string nome, out string retorno)
        {
            LastResult = Result;
            ResultCode = (short)PWRET.INVALID;
            retorno = "";
            try
            {
                ResultCode = Interop.PW_iPPDisplay(nome);
                Result = (PWRET)ResultCode;
                if (Result != PWRET.OK)
                    return false;

                uint ulDataSize = 100;
                var pszData = new StringBuilder((int)ulDataSize);
                var tempResult = (PWRET)Interop.PW_iPPEventLoop(pszData, ulDataSize);
                if (tempResult == PWRET.DISPLAY)
                    retorno = pszData.ToString();

                return true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("result", ResultCode);
                Erro = ex;
                return false;
            }
        }

        public bool Init() => dllInit;

        private bool Exec(Func<short> method)
        {
            LastResult = Result;
            ResultCode = (short)PWRET.INVALID;
            try
            {
                ResultCode = method();
                Result = (PWRET)ResultCode;
                return Result == PWRET.OK;
            }
            catch (Exception ex)
            {
                ex.Data.Add("result", ResultCode);
                Erro = ex;
                return false;
            }
        }

        public bool NewTransac(PWOPER operacao)
            => Exec(() => Interop.PW_iNewTransac((byte)operacao));

        public bool AddParam(PWINFO param, string cValor)
            => Exec(() => Interop.PW_iAddParam((ushort)param, cValor));

        public string GetResult(PWINFO info)
        {
            try
            {
                uint ulDataSize = 2048;
                var pszData = new StringBuilder((int)ulDataSize);
                var tempResult = Interop.PW_iGetResult((short)info, pszData, ulDataSize);
                if (tempResult == (short)PWRET.NODATA)
                    return "";

                return pszData.ToString();
            }
            catch (Exception ex)
            {
                Erro = ex;
                return "ERRO: " + ex.Message;
            }
        }

        public bool ExecTransac()
        {
            ResultCode = (short)PWRET.INVALID;
            try
            {
                short maxParam = Constantes.NUMPARAM;
                ResultCode = Interop.PW_iExecTransac(GetData, ref maxParam);
                Result = (PWRET)ResultCode;
                if (Result == PWRET.OK)
                    NumParam = 0;
                else
                    NumParam = maxParam;

                return true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("result", ResultCode);
                Erro = ex;
                return false;
            }
        }

        public PagamentoResultado ObterResultado(out string cFile)
        {
            PagamentoResultado pagamento = null;
            cFile = null;
            try
            {
                pagamento = new PagamentoResultado()
                {
                    Autorizacao = GetResult(PWINFO.AUTHCODE),
                    LocRef = GetResult(PWINFO.AUTLOCREF),
                    Bandeira = GetResult(PWINFO.CARDNAME),
                    ExtRef = GetResult(PWINFO.AUTEXTREF),
                    VirtMerch = GetResult(PWINFO.VIRTMERCH),
                    Debito = GetResult(PWINFO.CARDTYPE) == "2",
                    ContaRecebivel = GetResult(PWINFO.AUTHSYST),
                    ViaCliente = GetResult(PWINFO.RCPTCHSHORT), // RCPTCHSHORT, RCPTFULL
                    ViaEstabelecimento = GetResult(PWINFO.RCPTMERCH)
                };

                if (string.IsNullOrEmpty(pagamento.ViaCliente))
                    pagamento.ViaCliente = GetResult(PWINFO.RCPTFULL);

                if (string.IsNullOrEmpty(pagamento.ViaCliente))
                    pagamento.ViaCliente = GetResult(PWINFO.RCPTCHOLDER);

                cFile = Path.Combine(PayGoPath, string.Format(@"TEF\{0:yyyyMMdd-HHmmss}-{1}-{2}.AUT", DateTime.Now, pagamento.LocRef, pagamento.Autorizacao));

                var sb = new StringBuilder();

                // Dados para Confirmação
                sb.AppendLine($">REQNUM: {pagamento.Autorizacao}");
                sb.AppendLine($">AUTLOCREF: {pagamento.LocRef}");
                sb.AppendLine($">CARDNAME: {pagamento.Bandeira}");
                sb.AppendLine($">AUTEXTREF: {pagamento.ExtRef}");
                sb.AppendLine($">AUTHSYST: {pagamento.ContaRecebivel}");
                sb.AppendLine($">TOTAMNT: {GetResult(PWINFO.TOTAMNT)}");

                // Recibo
                sb.AppendLine($">RCPTCHSHORT: {pagamento.ViaCliente}");
                sb.AppendLine($">RCPTMERCH: {pagamento.ViaEstabelecimento}");

                File.WriteAllText(cFile, sb.ToString());

            }
            catch (Exception ex)
            {
                if (cFile != null)
                    MessageBox.Show("ERRO AO GRAVAR DADOS: " + cFile, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    throw ex;
            }
            return pagamento;
        }

        public static PagamentoResultado LerResultado(string cFile)
        {
            if (!File.Exists(cFile))
                return null;

            var pagamento = new PagamentoResultado();
            string dados = File.ReadAllText(cFile);

            var er = new System.Text.RegularExpressions.Regex(@">(\w+):\s(\w+)");
            var m = er.Match(dados);
            while (m.Success)
            {
                if (m.Groups.Count != 3)
                    return null;

                string chave = m.Groups[1].Value;
                string valor = m.Groups[2].Value;

                if (chave == "REQNUM")
                    pagamento.Autorizacao = valor;
                else if (chave == "AUTLOCREF")
                    pagamento.LocRef = valor;
                else if (chave == "CARDNAME")
                    pagamento.Bandeira = valor;
                else if (chave == "AUTEXTREF")
                    pagamento.ExtRef = valor;
                else if (chave == "AUTHSYST")
                    pagamento.ContaRecebivel = valor;

                m = m.NextMatch();
            }
            return pagamento;
        }

        public string Pendente()
        {
            var di = new DirectoryInfo(PayGoPath);
            foreach (var fi in di.GetFiles("*.AUT"))
                return fi.FullName;

            return null;
        }

        public ITipoDeDado CriaOperacao(ushort i)
        {
            return factoryOperation.Operacao(GetData, i);
        }

        public PWRET ExecutaOperacao(ITipoDeDado tipo, ushort i, ref string msg)
        {
            return (PWRET)tipo.Operacao(i, ref msg);
        }

        public bool Confirmation(PWCNF mode, PagamentoResultado dados, string cFile)
        {
            if (Exec(() => Interop.PW_iConfirmation(
                (uint)mode,
                dados.Autorizacao,
                dados.LocRef,
                dados.ExtRef,
                dados.VirtMerch,
                dados.ContaRecebivel)))
            {
                if (cFile != null)
                    File.Move(cFile, cFile.Substring(0, cFile.Length - 4) + ".CNF");

                return true;
            }
            else
            {
                if (cFile != null)
                    File.Move(cFile, cFile.Substring(0, cFile.Length - 4) + ".ERR");

                return false;
            }
        }
    }
}