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
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Servicos.ConsultaCadastro;
using NFe.Classes.Servicos.Status;
using NFe.Classes.Servicos.Tipos;
using NFe.Danfe.Base.NFCe;
using NFe.Danfe.Nativo.NFCe;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;
using NFe.Utils.NFe;
using System;
using System.Collections.Generic;

namespace a7D.PDV.ZeusNFCe
{
    public class NFeFacade
    {
        public ConfiguracaoCsc ConfigCsc { get; set; }

        public NFeFacade()
        {
            CarregarConfiguracoes();
            ConfigCsc = new ConfiguracaoCsc("000001", "ebc4a171-ed51-4880-9bf3-5ae7759f6444");
        }

        private void CarregarConfiguracoes()
        {
            //ConfiguracaoServico.Instancia.DiretorioSalvarXml = @"C:\Users\FábioFerreira\Downloads\XML-NFCe";
            //ConfiguracaoServico.Instancia.SalvarXmlServicos = true;
            ConfiguracaoServico.Instancia.cUF = Estado.SP;
            ConfiguracaoServico.Instancia.DiretorioSchemas = @"C:\Users\FábioFerreira\Google Drive\NFCe\PL_009_V4_2016_002_v160b";
            ConfiguracaoServico.Instancia.ModeloDocumento = ModeloDocumento.NFCe;
            ConfiguracaoServico.Instancia.TimeOut = 5000;
            ConfiguracaoServico.Instancia.tpAmb = TipoAmbiente.Homologacao;
            ConfiguracaoServico.Instancia.tpEmis = TipoEmissao.teOffLine;
            ConfiguracaoServico.Instancia.ProtocoloDeSeguranca = System.Net.SecurityProtocolType.Tls12;

            // Certificado!
            ConfiguracaoServico.Instancia.Certificado.TipoCertificado = TipoCertificado.A1Arquivo;
            ConfiguracaoServico.Instancia.Certificado.Arquivo = @"C:\Users\FábioFerreira\Google Drive\NFCe\13948439_out.pfx";
            ConfiguracaoServico.Instancia.Certificado.Senha = "12345678";
            ConfiguracaoServico.Instancia.Certificado.ManterDadosEmCache = true;

            var versaoNFe = VersaoServico.ve400;
            ConfiguracaoServico.Instancia.VersaoLayout = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfceAministracaoCSC = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNFeAutorizacao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaCadastro = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaDest = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaProtocolo = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNFeDistribuicaoDFe = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeDownloadNF = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeInutilizacao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeRecepcao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNFeRetAutorizacao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeRetRecepcao = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoNfeStatusServico = versaoNFe;
            ConfiguracaoServico.Instancia.VersaoRecepcaoEventoCceCancelamento = versaoNFe;
        }

        /// <summary>
        /// Consulta o status do serviço
        /// </summary>
        /// <returns></returns>
        public retConsStatServ ConsultarStatusServico()
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NfeStatusServico().Retorno;
        }

        /// <summary>
        /// Envia uma NFCe
        /// </summary>
        /// <param name="numLote"></param>
        /// <param name="nfe"></param>
        /// <returns></returns>
        public RetornoNFeAutorizacao EnviarNFe(Int32 numLote, NFe.Classes.NFe nfe)
        {
            nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NFeAutorizacao(numLote, IndicadorSincronizacao.Assincrono, new List<NFe.Classes.NFe> { nfe });
        }

        /// <summary>
        ///     Consulta a situação cadastral, com base na UF/Documento
        ///     <para>O documento pode ser: CPF ou CNPJ. O serviço avaliará o tamanho da string passada e determinará se a coonsulta será por CPF ou por CNPJ</para>
        /// </summary>
        /// <param name="uf">Sigla da UF consultada, informar 'SU' para SUFRAMA.</param>
        /// <param name="tipoDocumento">Tipo do documento</param>
        /// <param name="documento">CPF ou CNPJ</param>
        /// <returns>Retorna um objeto da classe RetornoNfeConsultaCadastro com o retorno do serviço NfeConsultaCadastro</returns>
        public RetornoNfeConsultaCadastro ConsultaCadastro(string uf, ConsultaCadastroTipoDocumento tipoDocumento,
            string documento)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NfeConsultaCadastro(uf, tipoDocumento, documento);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recibo"></param>
        /// <returns></returns>
        public RetornoNFeRetAutorizacao ConsultarReciboDeEnvio(string recibo)
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
        public RetornoRecepcaoEvento CancelarNFe(string cnpjEmitente, int numeroLote, short sequenciaEvento, string chaveAcesso,
            string protocolo, string justificativa)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.RecepcaoEventoCancelamento(numeroLote, sequenciaEvento, protocolo, chaveAcesso, justificativa, cnpjEmitente);
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
        public RetornoNfeInutilizacao InutilizarNumeracao(int ano, string cnpj, string justificativa,
            int numeroInicial, int numeroFinal, int serie)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            return servicoNFe.NfeInutilizacao(cnpj, Convert.ToInt16(ano.ToString().Substring(2, 2)), ConfiguracaoServico.Instancia.ModeloDocumento, Convert.ToInt16(serie), Convert.ToInt32(numeroInicial), Convert.ToInt32(numeroFinal), justificativa);
        }

        /// <summary>
        /// Imprime em um JPEG o NFC-e relacionado a um xml.
        /// </summary>
        /// <param name="pathXmlNFCe">Path do NFC-e a imprimir</param>
        /// <param name="pathPNG">Path onde gerar a imagem</param>
        public void ImprimirNFCe(string pathXmlNFCe, string pathPNG, string idToken, string csc)
        {
            var nfe = new NFe.Classes.NFe().CarregarDeArquivoXml(pathXmlNFCe);
            var arquivo = nfe.ObterXmlString();

            var configuracaoDanfeNFCe = new ConfiguracaoDanfeNfce(NFe.Danfe.Base.NfceDetalheVendaNormal.UmaLinha, NFe.Danfe.Base.NfceDetalheVendaContigencia.UmaLinha);
            DanfeNativoNfce impr = new DanfeNativoNfce(arquivo, configuracaoDanfeNFCe, idToken, csc);
            impr.GerarPNG(pathPNG);
        }
    }
}