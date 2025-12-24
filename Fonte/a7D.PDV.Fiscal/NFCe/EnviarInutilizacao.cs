using System;
using System.Text;
using System.Xml.Linq;
using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;

namespace a7D.PDV.Fiscal.NFCe
{
    public class EnviarInutilizacao : IEnviarInutilizacao
    {
        private readonly int _serie;
        private readonly int _numeroInicial;
        private readonly int _numeroFinal;
        private readonly string _motivo;
        private readonly int _idPdv;
        private readonly int _idUsuario;

        public EnviarInutilizacao(int serie, int numeroInicial, int numeroFinal, string motivo, int idPdv, int idUsuario)
        {
            _serie = serie;
            _numeroInicial = numeroInicial;
            _numeroFinal = numeroFinal;
            _motivo = (motivo ?? string.Empty).Trim();
            _idPdv = idPdv;
            _idUsuario = idUsuario;
        }

        public RetornoSATInformation Enviar()
        {
            Validar();

            int numeroSessao = GerarNumeroSessao();

            try
            {
                var client = FiscalServices.SatApiClient();

                var retornoStr = client
                    .InutilizacaoClient(_serie, _numeroInicial, _numeroFinal, _motivo, numeroSessao)
                    .Enviar();

                var retorno = MontarRetornoBasico(numeroSessao, retornoStr);

                return retorno;
            }
            catch (ExceptionPDV)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E520, ex, "Falha ao enviar inutilização.");
            }
        }

        private void Validar()
        {
            if (_serie <= 0)
                throw new ExceptionPDV(CodigoErro.E100, "Série inválida.");

            if (_numeroInicial <= 0)
                throw new ExceptionPDV(CodigoErro.E100, "Número inicial inválido.");

            if (_numeroFinal <= 0)
                throw new ExceptionPDV(CodigoErro.E100, "Número final inválido.");

            if (_numeroFinal < _numeroInicial)
                throw new ExceptionPDV(CodigoErro.E100, "O número final não pode ser menor que o número inicial.");

            if (_motivo.Length < 15)
                throw new ExceptionPDV(CodigoErro.E520, "A justificativa deve ter pelo menos 15 caracteres.");
        }

        private static int GerarNumeroSessao()
        {
            return (int)(DateTime.UtcNow.Ticks % int.MaxValue);
        }

        private RetornoSATInformation MontarRetornoBasico(int numeroSessao, string retornoStr)
        {
            var doc = XDocument.Parse(retornoStr);

            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
            var inf = doc.Root?.Element(ns + "infInut");

            string cStat  = inf?.Element(ns + "cStat")?.Value;
            string xMotivo = inf?.Element(ns + "xMotivo")?.Value;
            string nProt  = inf?.Element(ns + "nProt")?.Value;

            var xmlBytes = Encoding.UTF8.GetBytes(retornoStr);
            var xmlBase64 = Convert.ToBase64String(xmlBytes);

            return new RetornoSATInformation
            {
                TipoSolicitacaoSAT = new TipoSolicitacaoSATInformation
                {
                    IDTipoSolicitacaoSAT = (int)ETipoSolicitacaoSAT.INUTILIZAR_NUMERACAO
                },

                numeroSessao = numeroSessao.ToString(),
                timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"),

                EEEEE = cStat,               
                mensagem = xMotivo,          
                cod = nProt,                 

                arquivoCFeSAT = xmlBase64,
                mensagemSEFAZ = retornoStr,
            };
        }
        
        
    }
}
