using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;

namespace a7D.PDV.BLL
{
    public class TipoTributacao
    {
        public static List<TipoTributacaoInformation> Listar()
        {
            List<Object> listObj = CRUD.Listar(new TipoTributacaoInformation());
            List<TipoTributacaoInformation> list = listObj.ConvertAll(new Converter<Object, TipoTributacaoInformation>(TipoTributacaoInformation.ConverterObjeto));

            return list;
        }

        public static TipoTributacaoInformation Carregar(Int32 idTipoTributacao)
        {
            TipoTributacaoInformation obj = new TipoTributacaoInformation { IDTipoTributacao = idTipoTributacao };
            return (TipoTributacaoInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(TipoTributacaoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(TipoTributacaoInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(TipoTributacaoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idTipoTributacao)
        {
            TipoTributacaoInformation obj = new TipoTributacaoInformation { IDTipoTributacao = idTipoTributacao };
            CRUD.Excluir(obj);
        }
    }
}

