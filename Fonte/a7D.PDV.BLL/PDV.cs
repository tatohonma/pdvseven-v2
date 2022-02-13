using a7D.PDV.Ativacao.Shared.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace a7D.PDV.BLL
{
    public static class PDV
    {
        private static readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
        private static readonly TimeZoneInfo _brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public static void Salvar(PDVInformation pdv, bool verificar = true)
        {
            try
            {
                pdv.AtualizarDados(verificar);
                EF.Repositorio.Atualizar(pdv);
                //var e = pdv7Context.DB.Entry(pdv);
                //pdv7Context.DB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EB5B, ex);
            }
        }

        public static string Descriptografar(string scriptDBCript)
        {
            return CryptMD5.Descriptografar(scriptDBCript);
        }

        public static PDVInformation AtualizarDados(this PDVInformation pdv, bool validar = true)
        {
            if (pdv.IDPDV.HasValue == false && string.IsNullOrWhiteSpace(pdv.Dados))
            {
                //pdv.Dados = "placeholder";
                ////pdv7Context.DB.SaveChanges();
                //EF.Repositorio.Inserir(pdv);
                throw new ExceptionPDV(CodigoErro.EB80); // Nunca deve ocorrer!
            }
            else if (pdv.IDPDV.HasValue && string.IsNullOrWhiteSpace(pdv.Dados) == false)
            {
                var d = StringCipher.Decrypt(pdv.Dados).Split('|');
                if (Convert.ToInt32(d[0]) != pdv.IDPDV.Value && validar)
                    throw new ExceptionPDV(CodigoErro.EB5B);
            }

            var dados = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                pdv.IDPDV.Value,
                pdv.TipoPDV.IDTipoPDV,
                string.IsNullOrWhiteSpace(pdv.ChaveHardware) ? string.Empty : pdv.ChaveHardware,
                pdv.UltimoAcesso.HasValue ? pdv.UltimoAcesso.Value.ToUniversalTime().ToString(Ativacao.Shared.Services.PDVSecurity.dateformat, _cultureInfo) : "",
                pdv.UltimaAlteracao.HasValue ? pdv.UltimaAlteracao.Value.ToUniversalTime().ToString(Ativacao.Shared.Services.PDVSecurity.dateformat, _cultureInfo) : "",
                string.IsNullOrWhiteSpace(pdv.Versao) ? string.Empty : pdv.Versao,
                pdv.Ativo.HasValue ? (pdv.Ativo.Value ? 1 : 0) : 1);

            pdv.Dados = StringCipher.Encrypt(dados);
            return pdv;
        }

        public static void AlterarNome(int idPDV, string nome)
        {
            var pdv = Carregar(idPDV, null);
            if (pdv == null || string.IsNullOrWhiteSpace(pdv.Dados))
                throw new Exception("PDV com id " + idPDV + " não existe");

            pdv.Nome = nome;
            pdv.AtualizarDados();
            Salvar(pdv);
        }

        public static List<PDVInformation> Listar()
        {
            return EF.Repositorio.Listar<PDVInformation>().Where(p => p.CarregarDados()).ToList();
            //return pdv7Context.DB.tbPDVs.ToList().Where().ToList();
        }

        private static bool CarregarDados(this PDVInformation pdv)
        {
            if (string.IsNullOrWhiteSpace(pdv.Dados))
                return false;

            var dados = StringCipher.Decrypt(pdv.Dados).Split('|');

            if (Convert.ToInt32(dados[0]) != pdv.IDPDV)
                throw new ExceptionPDV(CodigoErro.EB5B, "IDPDV: " + pdv.IDPDV);

            int tipoPDV = Convert.ToInt32(dados[1]);
            pdv.TipoPDV = new TipoPDVInformation { IDTipoPDV = tipoPDV, Nome = ((ETipoPDV)tipoPDV).ToString() };
            pdv.ChaveHardware = dados[2];
            pdv.UltimoAcesso = ConverterDadosParaDateTime(dados[3]);
            pdv.UltimaAlteracao = ConverterDadosParaDateTime(dados[4]);
            pdv.Versao = dados[5];
            pdv.Ativo = dados.Length < 7 || Convert.ToInt32(dados[6]) == 1;

            return true;
        }

        public static PDVInformation Carregar(int idPDV, List<TipoPDVInformation> tipos = null)
        {
            try
            {
                var pdv = EF.Repositorio.Carregar<PDVInformation>(p => p.IDPDV == idPDV);
                //var pdv = pdv7Context.DB.tbPDVs.Find(idPDV);
                if (pdv == null)
                    return null;

                pdv.CarregarDados();
                if (tipos?.Count > 0)
                    pdv.TipoPDV = tipos.FirstOrDefault(t => t.IDTipoPDV == pdv?.TipoPDV?.IDTipoPDV);

                return pdv;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EB5B, ex);
            }
        }
        public static void Remover(int idPDV)
        {
            PDVInformation pdv = Carregar(idPDV, null);
            if (pdv.UltimaAlteracao != null && pdv.UltimaAlteracao.Value > DateTime.Now.AddHours(-48))
                throw new Exception("A substituição de equipamentos só pode ser realizada 1x a cada 48 horas!");

            pdv.ChaveHardware = null;
            pdv.UltimaAlteracao = DateTime.Now;

            Salvar(pdv);
        }

        private static bool? TemEstoque = null;
        public static bool PossuiEstoque()
        {
            if (TemEstoque == null)
                TemEstoque = PossuiTipo(ETipoPDV.CONTROLE_ESTOQUE);

            return TemEstoque.Value;
        }

        private static bool? TemTema = null;
        public static bool PossuiTema()
        {
            if (TemTema == null)
                TemTema = PossuiTipo(ETipoPDV.CARDAPIO_DIGITAL)
                       || PossuiTipo(ETipoPDV.AUTOATENDIMENTO)
                       || PossuiTipo(ETipoPDV.TORNEIRA);

            return TemTema.Value;
        }

        //private static bool? TemIaago = null;
        //public static bool PossuiIaago()
        //{
        //    if (TemIaago == null)
        //        TemIaago = PossuiTipo(ETipoPDV.IAAGO);

        //    return TemIaago.Value;
        //}

        private static bool? TemBigData = null;
        public static bool PossuiBigData()
        {
            if (TemBigData == null)
                TemBigData = PossuiTipo(ETipoPDV.BigData);

            return TemBigData.Value;
        }

        private static bool? TemPOSNTK = null;
        public static bool PossuiPOSNTK()
        {
            if (TemPOSNTK == null)
                TemPOSNTK = PossuiTipo(ETipoPDV.POS_INTEGRADO_NTK);

            return TemPOSNTK.Value;
        }

        private static bool? TemIFOOD= null;
        public static bool PossuiIFOOD()
        {
            if (TemIFOOD == null)
                TemIFOOD = PossuiTipo(ETipoPDV.IFOOD);

            return TemIFOOD.Value;
        }

        private static bool? TemEasyChopp = null;
        public static bool PossuiEasyChopp()
        {
            if (TemEasyChopp == null)
                TemEasyChopp = PossuiTipo(ETipoPDV.EASYCHOPP);

            return TemEasyChopp.Value;
        }

        private static bool? TemERP = null;
        public static bool PossuiERP()
        {
            if (TemERP == null)
                TemERP = PossuiTipo(ETipoPDV.ERP);

            return TemERP.Value;
        }

        private static bool? TemLoggi = null;
        public static bool PossuiLoggi()
        {
            if (TemLoggi == null)
                TemLoggi = PossuiTipo(ETipoPDV.LOGGI);

            return TemLoggi.Value;
        }

        private static bool PossuiTipo(ETipoPDV tipo)
        {
            return Listar().Any(p => p.TipoPDV.Tipo == tipo && p.Ativo == true);
        }

        private static DateTime? ConverterDadosParaDateTime(string dados)
        {
            if (string.IsNullOrWhiteSpace(dados))
                return null;
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.ParseExact(dados, Ativacao.Shared.Services.PDVSecurity.dateformat, _cultureInfo), _brasiliaTimeZone);
        }

        

        public static void AlterarDataValidade(DateTime validade)
        {
            ConfiguracaoBD.AlterarConfiguracaoSistema("dtValidade", CryptMD5.Criptografar(validade.ToString()));
        }

        public static bool VerificarAtivacaoOffline(string chave)
        {
            var chaveAtivacao = new ConfiguracoesAtivacao().ChaveAtivacao;
            if (string.IsNullOrWhiteSpace(chaveAtivacao))
                throw new Exception("Chave de ativação não encontrada");

            var result = a7D.PDV.Ativacao.Shared.Services.PDVSecurity.GerarAtivacaoOffline(chaveAtivacao);

            return string.Equals(chave, result, StringComparison.InvariantCultureIgnoreCase);
        }

       
    }
}
