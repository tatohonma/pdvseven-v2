using a7D.PDV.BLL;
using a7D.PDV.Model;

namespace a7D.PDV.Fiscal.SAT._007.Cancelamento
{
    public class CFeCancelamento
    {
        public static CFeCanc CarregarCFeCanc(RetornoSATInformation retornoSat, out PedidoInformation pedido)
        {
            var configuracoesSat = new ConfiguracoesSAT();
            CFeCanc cfeCanc = new CFeCanc();
            cfeCanc.infCFe = new _infCFe();
            cfeCanc.infCFe.chCanc = retornoSat.chaveConsulta;

            #region ide
            cfeCanc.infCFe.ide = new _ide();
            cfeCanc.infCFe.ide.CNPJ = configuracoesSat.InfCFe_ide_CNPJ;
            cfeCanc.infCFe.ide.signAC = configuracoesSat.InfCFe_ide_signAC;
            pedido = Pedido.CarregarPorIdRetornoSatVenda(retornoSat.IDRetornoSAT.Value);
            if (pedido != null)
                cfeCanc.infCFe.ide.numeroCaixa = pedido.Caixa.IDCaixa.Value.ToString().PadLeft(3, '0').Substring(0, 3);
            else
                cfeCanc.infCFe.ide.numeroCaixa = "000";
            #endregion

            #region emit
            cfeCanc.infCFe.emit = new _emit();
            #endregion

            #region dest
            cfeCanc.infCFe.dest = new _dest();
            #endregion

            #region total
            cfeCanc.infCFe.total = new _total();
            #endregion

            return cfeCanc;
        }
    }
}
