using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;

namespace a7D.PDV.BLL
{
    public static class Caixa
    {
        public static CaixaInformation Carregar(Int32 idCaixa)
        {
            CaixaInformation obj = new CaixaInformation { IDCaixa = idCaixa };
            obj = (CaixaInformation)CRUD.Carregar(obj);

            if (obj?.Usuario?.IDUsuario != null)
            {
                obj.Usuario = Usuario.Carregar(obj.Usuario.IDUsuario.Value);
            }

            return obj;
        }

        public static List<CaixaInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new CaixaInformation());
            List<CaixaInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaInformation>(CaixaInformation.ConverterObjeto));

            foreach (var item in lista)
            {
                item.PDV = PDV.Carregar(item.PDV.IDPDV.Value);
            }

            return lista;
        }

        public static int ContarAbertos()
        {
            return EF.Repositorio.Query<int?>("SELECT Count(1) FROM tbCaixa (nolock) WHERE DtFechamento IS NULL").FirstOrDefault() ?? 0;
        }

        public static IEnumerable<CaixaInformation> ListarAbertos()
        {
            foreach (var caixa in CaixaDAL.ListarAbertos())
            {
                if (caixa.PDV != null)
                    caixa.PDV = PDV.Carregar(caixa.PDV.IDPDV.Value);

                if (caixa.Usuario != null)
                    caixa.Usuario = Usuario.Carregar(caixa.Usuario.IDUsuario.Value);

                yield return caixa;
            }
        }

        public static IEnumerable<CaixaInformation> CaixasSemFechamento()
        {
            foreach (var caixa in CaixaDAL.CaixasSemFechamento())
            {
                if (caixa.PDV != null)
                    caixa.PDV = PDV.Carregar(caixa.PDV.IDPDV.Value);

                if (caixa.Usuario != null)
                    caixa.Usuario = Usuario.Carregar(caixa.Usuario.IDUsuario.Value);

                yield return caixa;
            }
        }

        public static List<CaixaInformation> ListarCompleto()
        {
            List<Object> listaObj = CRUD.Listar(new CaixaInformation());
            List<CaixaInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaInformation>(CaixaInformation.ConverterObjeto));

            foreach (var item in lista)
            {
                item.Usuario = Usuario.Carregar(item.Usuario.IDUsuario.Value);
                item.PDV = PDV.Carregar(item.PDV.IDPDV.Value);
            }

            return lista;
        }

        public static void ForcarFechamento(CaixaInformation caixa)
        {
            foreach (var registro in CaixaValorRegistro.ListarPorCaixa(caixa.IDCaixa.Value))
            {
                registro.ValorFechamento = Caixa.TotalMovimentacao(caixa.IDCaixa.Value, registro.TipoPagamento.IDTipoPagamento.Value);
                CaixaValorRegistro.Salvar(registro);
            }
            caixa.DtFechamento = DateTime.Now;
            BLL.Caixa.Salvar(caixa);
        }

        public static List<CaixaInformation> ListarPorFechamento(Int32 idFechamento)
        {
            CaixaInformation objFiltro = new CaixaInformation();
            objFiltro.Fechamento = new FechamentoInformation { IDFechamento = idFechamento };

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CaixaInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaInformation>(CaixaInformation.ConverterObjeto));

            return lista;
        }
        public static List<CaixaInformation> ListarPorPDV(Int32 idPDV)
        {
            CaixaInformation objFiltro = new CaixaInformation();
            objFiltro.PDV = new PDVInformation { IDPDV = idPDV };

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CaixaInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaInformation>(CaixaInformation.ConverterObjeto));

            return lista;
        }
        public static List<CaixaInformation> ListarNaoSincronizados()
        {
            CaixaInformation objFiltro = new CaixaInformation();
            objFiltro.SincERP = false;

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<CaixaInformation> lista = listaObj.ConvertAll(new Converter<Object, CaixaInformation>(CaixaInformation.ConverterObjeto));

            lista = lista.Where(l => l.DtFechamento != null).ToList();

            foreach (var item in lista)
            {
                item.PDV = PDV.Carregar(item.PDV.IDPDV.Value);
            }

            return lista;
        }

        public static void Salvar(CaixaInformation obj)
        {
            if (obj.IDCaixa == null)
            {
                Caixa.Adicionar(obj);
            }
            else
            {
                Caixa.Alterar(obj);
            }
        }
        public static void Adicionar(CaixaInformation obj)
        {
            CRUD.Adicionar(obj);
        }
        public static void Alterar(CaixaInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static CaixaInformation Status(Int32 idPDV)
        {
            var list = from l in Caixa.ListarPorPDV(idPDV)
                       orderby l.IDCaixa descending
                       select l;

            CaixaInformation caixa = list.FirstOrDefault();

            return caixa;
        }

        public static Decimal TotalMovimentacao(Int32 idCaixa, Int32 idTipoPagamento)
        {
            return CaixaDAL.TotalMovimentacao(idCaixa, idTipoPagamento);
        }

        public static void UsaOuAbreRefID(int idPDV, int idUsuario, ref int idCaixa)
        {
            if (idCaixa > 0)
                return;

            var caixa = UsaOuAbre(idPDV, idUsuario);
            idCaixa = caixa.IDCaixa.Value;
        }

        public static CaixaInformation UsaOuAbre(int idPDV, int idUsuario, int[] idTipoPagamento = null)
        {
            CaixaInformation caixa;
            var caixasAbertos = ListarAbertos().Where(c => c.PDV.IDPDV.Value == idPDV);

            if (caixasAbertos.Count() > 1)
                throw new ExceptionPDV(CodigoErro.E320);

            else if (caixasAbertos.Count() == 0)
            {
                var novoCaixa = new CaixaInformation
                {
                    DtAbertura = DateTime.Now,
                    PDV = new PDVInformation { IDPDV = idPDV },
                    Usuario = new UsuarioInformation() { IDUsuario = idUsuario },
                    SincERP = false
                };
                Caixa.Adicionar(novoCaixa);
                caixa = novoCaixa;
            }
            else
                caixa = caixasAbertos.FirstOrDefault();

            if (idTipoPagamento != null)
                ValidaTipoPagamento(caixa, idTipoPagamento);

            return caixa;
        }

        public static void ValidaTipoPagamento(CaixaInformation caixa, int[] idTipoPagamento)
        {
            var registros = CaixaValorRegistro.ListarPorCaixa(caixa.IDCaixa.Value);
            foreach (int idTipo in idTipoPagamento)
            {
                if (registros.Count(r => r.TipoPagamento.IDTipoPagamento == idTipo) == 0)
                {
                    var registro = new CaixaValorRegistroInformation
                    {
                        Caixa = caixa,
                        TipoPagamento = new TipoPagamentoInformation { IDTipoPagamento = idTipo },
                        ValorAbertura = 0
                    };
                    CaixaValorRegistro.Salvar(registro);
                }
            }
        }
    }
}
