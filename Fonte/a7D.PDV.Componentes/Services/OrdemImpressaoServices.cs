using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.Componentes.Services
{
    public static class OrdemImpressaoServices
    {
        public delegate void onLog(string info);

        private static DateTime dtErro = DateTime.Now;
        private static List<int> areaErro = new List<int>();

        public static string ImprimiPendente(int idPDV, string impressora, UsuarioInformation usuario, PDVInformation pdv, ConfiguracoesGerenciadorImpressao configOI)
        {
            var item = EF.Repositorio.CarregarConfig<tbOrdemImpressao>(
                    tb => tb.Include(nameof(tbOrdemImpressao.tbAreaImpressao)),
                    oi => oi.tbAreaImpressao.IDPDV == idPDV);

            if (item == null)
                return null;

            string info = Imprimir(impressora, item, usuario, pdv, configOI);
            EF.Repositorio.Excluir(item);
            return info;
        }

        public static void ImprimiPendentes(UsuarioInformation usuario, PDVInformation pdv, ConfiguracoesGerenciadorImpressao configOI, onLog AddLog)
        {
            var itens = EF.Repositorio.ListarConfig<tbOrdemImpressao>(
                    tb => tb.Include(nameof(tbOrdemImpressao.tbAreaImpressao)),
                    oi => oi.tbAreaImpressao.IDPDV == null);

            if (areaErro.Count > 0 && DateTime.Now.Subtract(dtErro).TotalMinutes > 1)
                // Tenta novamente a cada 1 minuto!
                areaErro.Clear();

            foreach (var item in itens)
            {
                try
                {
                    // Não imprime em áreas de produção marcadas com erro!
                    if (areaErro.Contains(item.IDAreaImpressao))
                        continue;

                    string log;
                    if (item.tbAreaImpressao?.NomeImpressora == null)
                        log = $"Ordem de impressao '{item.IDOrdemImpressao}' não contem área definida";
                    else
                        log =
                            item.tbAreaImpressao?.Nome + " - " +
                            item.tbAreaImpressao?.NomeImpressora +
                            " : Imprimindo " +
                            Imprimir(item.tbAreaImpressao?.NomeImpressora, item, usuario, pdv, configOI);

                    AddLog(log);
                    EF.Repositorio.Excluir(item);
                }
                catch (Exception ex)
                {
                    var info = $"{item.IDOrdemImpressao}: {item.tbAreaImpressao?.Nome} - {item.tbAreaImpressao?.NomeImpressora}";
                    var exPDV = new ExceptionPDV(CodigoErro.A310, ex, info);
                    AddLog(exPDV.Message);

                    dtErro = DateTime.Now;
                    if (!areaErro.Contains(item.IDAreaImpressao))
                        areaErro.Add(item.IDAreaImpressao);
                }
            }
        }

        private static string Imprimir(string impressora, tbOrdemImpressao item, UsuarioInformation usuario, PDVInformation pdv, ConfiguracoesGerenciadorImpressao configOI)
        {
            string log;
            if (item.TipoOrdemImpressao == ETipoOrdemImpressao.Producao)
            {
                log = "Imprimindo Ordem de Produção";
                ImpressoraWindows.ImprimirTexto(impressora, false, item.ConteudoImpressao, true);
            }
            else if (item.TipoOrdemImpressao == ETipoOrdemImpressao.Conta)
            {
                var conteudo = item.ConteudoImpressao?.Split('|');
                var pedido = Pedido.CarregarCompleto(Convert.ToInt32(conteudo[0]));

                if (conteudo.Length == 3)
                {
                    usuario = Usuario.Carregar(Convert.ToInt32(conteudo[1]));
                    pdv = BLL.PDV.Carregar(Convert.ToInt32(conteudo[2]));
                }

                log = "Imprimindo Conta";
                Pedido.AdicionarProdutoServico(pedido, true, pdv, usuario);
                Pedido.AdicionarProdutoConsumacaoMinima(pedido, pdv, usuario);
                ContaServices.ImprimirConta(impressora, pedido);
            }
            else if (item.TipoOrdemImpressao == ETipoOrdemImpressao.SAT)
            {
                log = "Imprimindo SAT";
                var conteudo = item.ConteudoImpressao?.Split('|');
                if (conteudo.Length == 2)
                {
                    CupomSATService.ImprimirCupomVenda(conteudo[0], Convert.ToInt32(conteudo[1]), impressora, out Exception ex);
                    if (ex != null)
                        throw ex;
                }
            }
            else if (item.TipoOrdemImpressao == ETipoOrdemImpressao.Expedicao)
            {
                log = "Imprimindo via de Expedicao";
                var conteudo = item.ConteudoImpressao?.Split('|');

                var pedido = Pedido.CarregarCompleto(Convert.ToInt32(conteudo[1]));
                if (!ExpedicaoServices.ImprimirVia(impressora, pedido))
                {
                    return log = "";
                }

                if (pedido.TipoPedido.TipoPedido == EF.Enum.ETipoPedido.Delivery)
                {
                    if (ConfiguracoesSistema.Valores.ImprimirViaControleDelivery)
                    {
                        var linhas = ExpedicaoServices.ComprovanteEntrega(pedido);
                        ImpressoraWindows.ImprimirTexto(impressora, false, String.Join("\r\n", linhas));
                    }

                    if (pedido.RetornoSAT_venda?.IDRetornoSAT > 0)
                    {
                        pedido.RetornoSAT_venda = RetornoSAT.Carregar(pedido.RetornoSAT_venda.IDRetornoSAT.Value);
                        CupomSATService.ImprimirCupomVenda(pedido.RetornoSAT_venda.arquivoCFeSAT, pedido, impressora, out Exception ex, out byte[] img, false, Constantes.TotalWidth);
                    }
                }
            }
            else
                throw new InvalidOperationException("Tipo ordem de impressão inválido: " + item.IDTipoOrdemImpressao);

            return log;
        }
    }
}
