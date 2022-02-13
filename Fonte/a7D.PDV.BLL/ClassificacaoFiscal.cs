using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
namespace a7D.PDV.BLL
{
    public class ClassificacaoFiscal
    {
        public static List<ClassificacaoFiscalInformation> Listar()
        {
            List<object> listObj = CRUD.Listar(new ClassificacaoFiscalInformation());
            List<ClassificacaoFiscalInformation> list = listObj.Cast<ClassificacaoFiscalInformation>().ToList();

            return list;
        }

        public static ClassificacaoFiscalInformation Carregar(Int32 idClassificacaoFiscal)
        {
            ClassificacaoFiscalInformation obj = new ClassificacaoFiscalInformation { IDClassificacaoFiscal = idClassificacaoFiscal };
            var resultado = (ClassificacaoFiscalInformation)CRUD.Carregar(obj);
            resultado.TipoTributacao = TipoTributacao.Carregar(resultado.TipoTributacao.IDTipoTributacao.Value);
            return resultado;
        }



        public static void Salvar(ClassificacaoFiscalInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(ClassificacaoFiscalInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(ClassificacaoFiscalInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idClassificacaoFiscal)
        {
            ClassificacaoFiscalInformation obj = new ClassificacaoFiscalInformation { IDClassificacaoFiscal = idClassificacaoFiscal };
            CRUD.Excluir(obj);
        }

        public static List<ClassificacaoFiscalInformation> ListarCompleto()
        {
            var list = Listar();

            foreach (var item in list)
                item.TipoTributacao = TipoTributacao.Carregar(item.TipoTributacao.IDTipoTributacao.Value);

            return list;
        }
    }
}

