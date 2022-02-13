using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class TipoPedido
    {
        public static List<TipoPedidoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new TipoPedidoInformation());
            List<TipoPedidoInformation> lista = listaObj.ConvertAll(new Converter<Object, TipoPedidoInformation>(TipoPedidoInformation.ConverterObjeto));

            return lista;
        }

        public static TipoPedidoInformation Carregar(Int32 idTipoPedido)
        {
            TipoPedidoInformation obj = new TipoPedidoInformation { IDTipoPedido = idTipoPedido };
            obj = (TipoPedidoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static Decimal RetornarTaxaServico(ETipoPedido tipoPedido)
        {
            switch (tipoPedido)
            {
                case ETipoPedido.Mesa:
                    return Convert.ToDecimal(ConfiguracoesSistema.Valores.TaxaServicoMesa);

                case ETipoPedido.Comanda:
                    return Convert.ToDecimal(ConfiguracoesSistema.Valores.TaxaServicoComanda);

                case ETipoPedido.Delivery:
                    return Convert.ToDecimal(ConfiguracoesSistema.Valores.TaxaServicoEntrega);

                case ETipoPedido.Balcao:
                    return Convert.ToDecimal(ConfiguracoesSistema.Valores.TaxaServicoBalcao);

                default:
                    throw new ExceptionPDV(CodigoErro.EC40);
            }
        }
    }
}
