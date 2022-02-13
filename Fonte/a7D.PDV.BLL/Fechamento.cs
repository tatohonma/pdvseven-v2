using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.BLL
{
    public class Fechamento
    {
        public static List<FechamentoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new FechamentoInformation());
            List<FechamentoInformation> lista = listaObj.ConvertAll(new Converter<Object, FechamentoInformation>(FechamentoInformation.ConverterObjeto));

            return lista;
        }

        public static FechamentoInformation Carregar(Int32 idFechamento)
        {
            FechamentoInformation obj = new FechamentoInformation { IDFechamento = idFechamento };
            obj = (FechamentoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(FechamentoInformation fechamento)
        {
            CRUD.Salvar(fechamento);
        }

        public static void Excluir(Int32 idFechamento)
        {
            try
            {
                FechamentoInformation obj = new FechamentoInformation { IDFechamento = idFechamento };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static FechamentoInformation Fechar(Int32 idUsuario, Int32 idPDV)
        {
            var fechamento = new FechamentoInformation();
            var listaCaixa = Caixa.CaixasSemFechamento().ToList();

            fechamento.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
            fechamento.PDV = new PDVInformation { IDPDV = idPDV };
            fechamento.DtFechamento = DateTime.Now;

            Salvar(fechamento);

            foreach (var item in listaCaixa)
            {
                item.Fechamento = fechamento;
                Caixa.Salvar(item);
            }
            return fechamento;
        }

        public static int UltimoFechamento()
        {
            return DAL.FechamentoDAL.UltimoFechamento();
        }

        public static FechamentoInformation CarregarCompleto(int idFechamento)
        {
            FechamentoInformation obj = new FechamentoInformation { IDFechamento = idFechamento };
            obj = (FechamentoInformation)CRUD.Carregar(obj);

            if (obj.Usuario != null)
                obj.Usuario = Usuario.Carregar(obj.Usuario.IDUsuario.Value);

            obj.PDV = PDV.Carregar(obj.PDV.IDPDV.Value);

            return obj;
        }
    }
}
