/********************************************************************************/
/* Projeto: Biblioteca ZeusNFe                                                  */
/* Biblioteca C# para emissão de Nota Fiscal Eletrônica - NFe e Nota Fiscal de  */
/* Consumidor Eletrônica - NFC-e (http://www.nfe.fazenda.gov.br)                */
/*                                                                              */
/* Direitos Autorais Reservados (c) 2014 Adenilton Batista da Silva             */
/*                                       Zeusdev Tecnologia LTDA ME             */
/*                                                                              */
/*  Você pode obter a última versão desse arquivo no GitHub                     */
/* localizado em https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe               */
/*                                                                              */
/*                                                                              */
/*  Esta biblioteca é software livre; você pode redistribuí-la e/ou modificá-la */
/* sob os termos da Licença Pública Geral Menor do GNU conforme publicada pela  */
/* Free Software Foundation; tanto a versão 2.1 da Licença, ou (a seu critério) */
/* qualquer versão posterior.                                                   */
/*                                                                              */
/*  Esta biblioteca é distribuída na expectativa de que seja útil, porém, SEM   */
/* NENHUMA GARANTIA; nem mesmo a garantia implícita de COMERCIABILIDADE OU      */
/* ADEQUAÇÃO A UMA FINALIDADE ESPECÍFICA. Consulte a Licença Pública Geral Menor*/
/* do GNU para mais detalhes. (Arquivo LICENÇA.TXT ou LICENSE.TXT)              */
/*                                                                              */
/*  Você deve ter recebido uma cópia da Licença Pública Geral Menor do GNU junto*/
/* com esta biblioteca; se não, escreva para a Free Software Foundation, Inc.,  */
/* no endereço 59 Temple Street, Suite 330, Boston, MA 02111-1307 USA.          */
/* Você também pode obter uma copia da licença em:                              */
/* http://www.opensource.org/licenses/lgpl-license.php                          */
/*                                                                              */
/* Zeusdev Tecnologia LTDA ME - adenilton@zeusautomacao.com.br                  */
/* http://www.zeusautomacao.com.br/                                             */
/* Rua Comendador Francisco josé da Cunha, 111 - Itabaiana - SE - 49500-000     */
/********************************************************************************/
using a7D.PDV.BLL;
using DFe.Utils;
using NFe.Classes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Protocolo;
using NFe.Classes.Servicos.ConsultaCadastro;
using NFe.Classes.Servicos.Status;
using NFe.Classes.Servicos.Tipos;
using NFe.Danfe.Base.NFCe;
using NFe.Danfe.Nativo.NFCe;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;
using NFe.Utils.InformacoesSuplementares;
using NFe.Utils.NFe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace a7D.PDV.Fiscal.NFCe
{
    public static class NFeFacade
    {
        public static readonly ConfiguracoesNFCe Config;
        private static ConfiguracaoDanfeNfce configuracaoDanfeNFCe;
        private static readonly string HomologacaoNFCe = ConfigurationManager.AppSettings["HomologacaoNFCe"];

        static NFeFacade()
        {
            try
            {
                Config = new ConfiguracoesNFCe();
                CarregarConfiguracoes();
            }
            catch (Exception ex)
            {
                Logs.Erro(CodigoErro.E514, ex);
                throw ex;
            }
        }

        public static void ConfigPathXSD(string pathDLL)
        {
            ConfiguracaoServico.Instancia.DiretorioSchemas = Path.Combine(pathDLL, "XSD");
        }

        private static void CarregarConfiguracoes()
        {
            if (string.IsNullOrEmpty(Config.NFCe_CertificadoArquivo))
                return;

            ConfiguracaoServico.Instancia.cUF = (DFe.Classes.Entidades.Estado)Config.NFCe_UF;
            Config.xNFCe_UF = ConfiguracaoServico.Instancia.cUF.ToString();

            ConfiguracaoServico.Instancia.ModeloDocumento = DFe.Classes.Flags.ModeloDocumento.NFCe;
            ConfiguracaoServico.Instancia.TimeOut = 1000 * Config.NFCe_Timeout;
            ConfiguracaoServico.Instancia.tpEmis = TipoEmissao.teNormal;
            ConfiguracaoServico.Instancia.ProtocoloDeSeguranca = System.Net.SecurityProtocolType.Tls12;

            if (!string.IsNullOrEmpty(HomologacaoNFCe) && Boolean.TryParse(HomologacaoNFCe, out bool homolog) && homolog)
                ConfiguracaoServico.Instancia.tpAmb = DFe.Classes.Flags.TipoAmbiente.Homologacao;
            else
                ConfiguracaoServico.Instancia.tpAmb = DFe.Classes.Flags.TipoAmbiente.Producao;

            // Certificado!
            ConfiguracaoServico.Instancia.Certificado.TipoCertificado = TipoCertificado.A1Arquivo;
            ConfiguracaoServico.Instancia.Certificado.Arquivo = Config.NFCe_CertificadoArquivo;
            ConfiguracaoServico.Instancia.Certificado.Senha = Config.NFCe_CertificadoSenha;
            ConfiguracaoServico.Instancia.Certificado.ManterDadosEmCache = true;

            var versaoNFe = VersaoServico.ve400;
            ConfiguracaoServico.Instancia.VersaoLayout = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoRecepcaoEventoCceCancelamento = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoRecepcaoEventoEpec = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoRecepcaoEventoManifestacaoDestinatario = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoNfeRecepcao = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoNfeRetRecepcao = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaCadastro = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoNfeInutilizacao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaProtocolo = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeStatusServico = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNFeAutorizacao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNFeRetAutorizacao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNFeDistribuicaoDFe = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaDest = VersaoServico.ve100;
            ConfiguracaoServico.Instancia.VersaoNfeDownloadNF = VersaoServico.ve310;
            ConfiguracaoServico.Instancia.VersaoNfceAministracaoCSC = VersaoServico.ve100;

            configuracaoDanfeNFCe = new ConfiguracaoDanfeNfce(NFe.Danfe.Base.NfceDetalheVendaNormal.UmaLinha, NFe.Danfe.Base.NfceDetalheVendaContigencia.UmaLinha);
            //using (var ms = new MemoryStream())
            //{
            //    var img = ImageUtil.LogoPDV7_Horizontal_PB();
            //    img.Save(ms, ImageFormat.Png);
            //    configuracaoDanfeNFCe.Logomarca = ms.GetBuffer();
            //}
        }

        public static string EnviarAssinada(string xml)
        {
            var nfe = new NFe.Classes.NFe();
            nfe.CarregarDeXmlString(xml);
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            var retorno = servicoNFe.NFeAutorizacao(Config.Lote, IndicadorSincronizacao.Assincrono, new List<NFe.Classes.NFe> { nfe });
            return retorno.RetornoCompletoStr;
        }

        public static void NFeProcObterDadosXML(string xml,
            out int modelo, out int serie, out long nNF, out int CFOP, out DateTime dtEmissao,
            out decimal valorTotal, out decimal baseCalcICMS, out decimal vlrICMS)
        {
            var nfeProc = new nfeProc().CarregarDeXmlString(xml);

            modelo = (int)nfeProc.NFe.infNFe.ide.mod;
            serie = nfeProc.NFe.infNFe.ide.serie;
            nNF = nfeProc.NFe.infNFe.ide.nNF;
            dtEmissao = nfeProc.NFe.infNFe.ide.dEmi;
            CFOP = nfeProc.NFe.infNFe.det[0].prod.CFOP;
            valorTotal = nfeProc.NFe.infNFe.total.ICMSTot.vNF;
            baseCalcICMS = nfeProc.NFe.infNFe.total.ICMSTot.vNF;
            vlrICMS = nfeProc.NFe.infNFe.total.ICMSTot.vICMS;
        }

        /// <summary>
        /// Consulta o status do serviço
        /// </summary>
        /// <returns></returns>
        public static retConsStatServ ConsultarStatusServico()
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NfeStatusServico().Retorno;
        }

        /// <summary>
        /// Assina a NFCe
        /// </summary>
        /// <param name="nfe"></param>
        /// <returns></returns>
        public static NFe.Classes.NFe Assinar(NFe.Classes.NFe nfe)
        {
            return nfe.Assina();
        }

        /// <summary>
        /// Valida a NFCe
        /// </summary>
        /// <param name="nfe"></param>
        /// <returns></returns>
        public static NFe.Classes.NFe Validar(NFe.Classes.NFe nfe)
        {
            nfe.infNFeSupl = new infNFeSupl()
            {
                urlChave = nfe.infNFeSupl.ObterUrlConsulta(nfe, VersaoQrCode.QrCodeVersao2),
                qrCode = nfe.infNFeSupl.ObterUrlQrCode(nfe, VersaoQrCode.QrCodeVersao2, Config.NFCe_CIdToken, Config.NFCe_CSC)
            };

            return nfe.Valida();
        }

        /// <summary>
        /// Envia uma NFCe
        /// </summary>
        /// <param name="nfe"></param>
        /// <returns></returns>
        public static RetornoNFeAutorizacao Enviar(NFe.Classes.NFe nfe)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NFeAutorizacao(Config.Lote, IndicadorSincronizacao.Sincrono, new List<NFe.Classes.NFe> { nfe });
        }

        public static string EmitirOffline(NFe.Classes.NFe nfe, string justificativa)
        {
            nfe.infNFe.ide.tpEmis = TipoEmissao.teOffLine;
            nfe.infNFe.ide.dhCont = DateTime.Now;
            nfe.infNFe.ide.xJust = justificativa;
            nfe.infNFe.ide.finNFe = FinalidadeNFe.fnNormal;
            nfe.infNFe.ide.indFinal = ConsumidorFinal.cfConsumidorFinal;
            nfe.infNFe.ide.indPres = PresencaComprador.pcPresencial;
            Assinar(nfe);
            Validar(nfe);
            return nfe.ObterXmlString();
        }

        /// <summary>
        /// Obtem o Protocolo de Consulta
        /// </summary>
        /// <param name="nfe"></param>
        /// <returns></returns>
        public static nfeProc Consulta(string chave)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            var retornoConsulta = servicoNFe.NfeConsultaProtocolo(chave);
            return new nfeProc
            {
                //NFe = nfe,
                protNFe = retornoConsulta.Retorno.protNFe,
                versao = retornoConsulta.Retorno.versao
            };
        }

        /// <summary>
        ///     Consulta a situação cadastral, com base na UF/Documento
        ///     <para>O documento pode ser: CPF ou CNPJ. O serviço avaliará o tamanho da string passada e determinará se a coonsulta será por CPF ou por CNPJ</para>
        /// </summary>
        /// <param name="uf">Sigla da UF consultada, informar 'SU' para SUFRAMA.</param>
        /// <param name="tipoDocumento">Tipo do documento</param>
        /// <param name="documento">CPF ou CNPJ</param>
        /// <returns>Retorna um objeto da classe RetornoNfeConsultaCadastro com o retorno do serviço NfeConsultaCadastro</returns>
        public static RetornoNfeConsultaCadastro ConsultaCadastro()
        {
            var uf = ConfiguracaoServico.Instancia.cUF.ToString();
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NfeConsultaCadastro(uf, ConsultaCadastroTipoDocumento.Cnpj, "CNPJ");
        }

        /// <summary>
        /// Consulta recibo de envio
        /// </summary>
        /// <param name="recibo"></param>
        /// <returns></returns>
        public static RetornoNFeRetAutorizacao ConsultarReciboDeEnvio(string recibo)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NFeRetAutorizacao(recibo);
        }

        /// <summary>
        /// Cancela uma NFCe
        /// </summary>
        /// <param name="cnpjEmitente"></param>
        /// <param name="numeroLote"></param>
        /// <param name="sequenciaEvento"></param>
        /// <param name="chaveAcesso"></param>
        /// <param name="protocolo"></param>
        /// <param name="justificativa"></param>
        /// <returns></returns>
        public static RetornoRecepcaoEvento Cancelar(protNFe protNFe, string justificativa)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.RecepcaoEventoCancelamento(Config.Lote, 1, protNFe.infProt.nProt, protNFe.infProt.chNFe, justificativa, Config.NFCe_CNPJ);
        }

        /// <summary>
        /// Inutiliza um range de NFCe
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="cnpj"></param>
        /// <param name="justificativa"></param>
        /// <param name="numeroInicial"></param>
        /// <param name="numeroFinal"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        public static RetornoNfeInutilizacao InutilizarNumeracao(int ano, string cnpj, string justificativa, int numeroInicial, int numeroFinal, int serie)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NfeInutilizacao(cnpj, Convert.ToInt16(ano.ToString().Substring(2, 2)), ConfiguracaoServico.Instancia.ModeloDocumento, Convert.ToInt16(serie), Convert.ToInt32(numeroInicial), Convert.ToInt32(numeroFinal), justificativa);
        }

        /// <summary>
        /// Imprime em um JPEG o NFC-e relacionado a um xml.
        /// </summary>
        /// <param name="arquivo">Conteudo XML</param>
        /// <param name="pathPNG">Path onde gerar a imagem</param>
        public static void Imprimir(string arquivo, string pathPNG)
        {
            var configuracaoDanfeNFCe = new ConfiguracaoDanfeNfce(NFe.Danfe.Base.NfceDetalheVendaNormal.UmaLinha, NFe.Danfe.Base.NfceDetalheVendaContigencia.UmaLinha);
            DanfeNativoNfce impr = new DanfeNativoNfce(arquivo, configuracaoDanfeNFCe, Config.NFCe_CIdToken, Config.NFCe_CSC);

            impr.GerarPNG(filename: pathPNG);
        }

        public static bool Imprimir(string arquivo, string modeloImpressora, out Exception mensagemErro, out byte[] image, bool makeImage, int totalWidth)
        {
            try
            {

                DanfeNativoNfce impr = new DanfeNativoNfce(arquivo, configuracaoDanfeNFCe, Config.NFCe_CIdToken, Config.NFCe_CSC);

                if (makeImage)
                {
                    using (var ms = new MemoryStream())
                    {
                        impr.GerarPNG(totalWidth, sw: ms);
                        image = ms.GetBuffer();
                    }
                }
                else
                {
                    impr.Imprimir(modeloImpressora);
                    image = null;
                }
                mensagemErro = null;
                return true;
            }
            catch (Exception ex)
            {
                image = null;
                mensagemErro = ex;
                return false;
            }
        }

        public static ide GetIdentificacao(int numero)
        {
            var ide = new ide
            {
                cUF = ConfiguracaoServico.Instancia.cUF,
                natOp = "VENDA",
                mod = ConfiguracaoServico.Instancia.ModeloDocumento,
                serie = Config.NFCe_Serie,
                nNF = numero,
                tpNF = TipoNFe.tnSaida,
                cMunFG = Config.NFCe_MuninipioIBGE,
                tpEmis = ConfiguracaoServico.Instancia.tpEmis,
                tpImp = TipoImpressao.tiNFCe,
                cNF = numero.ToString(),
                tpAmb = ConfiguracaoServico.Instancia.tpAmb,
                finNFe = FinalidadeNFe.fnNormal,
                verProc = "3.4.5.6",
                procEmi = ProcessoEmissao.peAplicativoContribuinte,
                indFinal = ConsumidorFinal.cfConsumidorFinal, //NFCe: Tem que ser consumidor Final
                indPres = PresencaComprador.pcPresencial, //NFCe: deve ser 1 ou 4
                idDest = DestinoOperacao.doInterna,
                dhEmi = DateTime.Now
            };

            if (ide.tpEmis != TipoEmissao.teNormal)
            {
                ide.dhCont = DateTime.Now;
                ide.xJust = "TESTE DE CONTIGÊNCIA PARA NFe/NFCe";
            }

            return ide;
        }

        public static emit GetEmitente()
        {
            return new emit
            {
                CNPJ = Config.NFCe_CNPJ,
                xNome = Config.NFCe_RazaoScocial,
                xFant = Config.NFCe_Fantasia,
                IE = Config.NFCe_IE,
                CPF = Config.NFCe_CPF,
                CRT = (CRT)Config.NFCe_CRT,
                enderEmit = GetEnderecoEmitente()
            };
        }

        internal static enderDest GetEnderecoDestinatario()
        {
            return new enderDest
            {
                xLgr = Config.NFCe_Logradouro,
                nro = Config.NFCe_Numero,
                xCpl = Config.NFCe_Complemento,
                xBairro = Config.NFCe_Bairro,
                cMun = Config.NFCe_MuninipioIBGE,
                xMun = Config.NFCe_Municipio,
                UF = Config.xNFCe_UF,
                CEP = Config.NFCe_CEP.ToString().PadLeft(8, '0'),
                fone = Config.NFCe_Telefone,
                cPais = 1058,
                xPais = "BRASIL"
            };
        }

        internal static enderEmit GetEnderecoEmitente()
        {
            return new enderEmit
            {
                xLgr = Config.NFCe_Logradouro,
                nro = Config.NFCe_Numero,
                xCpl = Config.NFCe_Complemento,
                xBairro = Config.NFCe_Bairro,
                cMun = Config.NFCe_MuninipioIBGE,
                xMun = Config.NFCe_Municipio,
                UF = Config.xNFCe_UF,
                CEP = Config.NFCe_CEP.ToString().PadLeft(8, '0'),
                fone = Config.NFCe_Telefone,
                cPais = 1058,
                xPais = "BRASIL"
            };
        }
    }
}