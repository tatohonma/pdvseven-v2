using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class Tag
    {
        public static TagInformation Carregar(string guidIdentificacao, string chave)
        {
            TagInformation tag = new TagInformation();
            tag.GUIDIdentificacao = guidIdentificacao;
            tag.Chave = chave;
            CRUD.Carregar(tag);

            return tag;
        }

        public static TagInformation CarregarPorChaveValor(string chave, string valor)
        {
            TagInformation tag = new TagInformation();
            tag.Chave = chave;
            tag.Valor = valor;
            CRUD.Carregar(tag);

            return tag;
        }

        public static TagInformation Adicionar(string guidIdentificacao, string chave, string valor)
        {
            TagInformation tag = new TagInformation();
            tag.GUIDIdentificacao = guidIdentificacao;
            tag.Chave = chave;
            tag.Valor = valor;
            tag.DtInclusao = DateTime.Now;
            CRUD.Adicionar(tag);

            return tag;
        }

        public static TagInformation Alterar(string guidIdentificacao, string chave, string valor)
        {
            TagInformation tag = new TagInformation();
            tag.GUIDIdentificacao = guidIdentificacao;
            tag.Chave = chave;
            CRUD.Carregar(tag);

            tag.Valor = valor;
            CRUD.Alterar(tag);

            return tag;
        }

        public static void Excluir(Int32 idTag)
        {
            TagInformation tag = new TagInformation();
            tag.IDTag = idTag;
            CRUD.Excluir(tag);
        }

        public static List<TagInformation> Listar(string guidIdentificacao)
        {
            TagInformation tag = new TagInformation();
            tag.GUIDIdentificacao = guidIdentificacao;

            List<Object> listaObj = CRUD.Listar(tag);
            List<TagInformation> lista = listaObj.ConvertAll(new Converter<Object, TagInformation>(TagInformation.ConverterObjeto));

            return lista;
        }

        public static List<String> ListarChaves()
        {
            return DAL.TagDAL.ListaChaves();
        }
    }
}
