using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class ConversaoUnidade
    {
        public static List<ConversaoUnidadeInformation> Listar()
        {
            List<ConversaoUnidadeInformation> list = CRUD.Listar(new ConversaoUnidadeInformation()).Cast<ConversaoUnidadeInformation>().ToList();
            list.ForEach(c =>
            {
                c.Unidade_de = Unidade.Carregar(c.Unidade_de.IDUnidade.Value);
                c.Unidade_para = Unidade.Carregar(c.Unidade_para.IDUnidade.Value);
            });
            return list;
        }

        public static ConversaoUnidadeInformation Carregar(int idConversaoUnidade)
        {
            ConversaoUnidadeInformation obj = new ConversaoUnidadeInformation { IDConversaoUnidade = idConversaoUnidade };
            return (ConversaoUnidadeInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(ConversaoUnidadeInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(ConversaoUnidadeInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(ConversaoUnidadeInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(int idConversaoUnidade)
        {
            ConversaoUnidadeInformation obj = new ConversaoUnidadeInformation { IDConversaoUnidade = idConversaoUnidade };
            CRUD.Excluir(obj);
        }

        public static void Excluir(ConversaoUnidadeInformation unidade)
        {
            if (unidade.IDConversaoUnidade.HasValue)
                Excluir(unidade.IDConversaoUnidade.Value);
        }

        public static List<UnidadeInformation> ListarUnidadesConversaoPara(UnidadeInformation unidade)
        {
            return ListarConversoes(unidade.IDUnidade.Value);
        }

        public static List<UnidadeInformation> ListarConversoes(int idUnidade)
        {
            return Listar()
                .Where(c => c.Unidade_de.IDUnidade == idUnidade)
                .Select(c => c.Unidade_para)
                .Distinct()
                .ToList();
        }

        public static List<UnidadeInformation> ListarTodasConversoes(UnidadeInformation unidade)
        {
            var conversoesDe = ListarConversoesDe(unidade).Select(c => c.Unidade_para);
            var conversoesPara = ListarConversoesPara(unidade).Select(c => c.Unidade_de);
            var todasConversoes = conversoesDe.Union(conversoesPara).ToList();
            todasConversoes = todasConversoes.DistinctBy(c => c.IDUnidade.Value).ToList();

            return todasConversoes;
        }

        public static decimal Converter(decimal quantidade, int idUnidadeDe, int? idUnidadePara = null, int? idProduto = null, bool exata = false)
        {
            var unidadeDe = Unidade.Carregar(idUnidadeDe);
            UnidadeInformation unidadePara = null;
            ProdutoInformation produto = null;

            if (idUnidadePara.HasValue)
            {
                unidadePara = Unidade.Carregar(idUnidadePara.Value);
                return Converter(quantidade, unidadeDe, unidadePara, exata);
            }

            if (idProduto.HasValue)
            {
                produto = Produto.Carregar(idProduto.Value);
                produto.Unidade = Unidade.Carregar(produto.Unidade.IDUnidade.Value);
                return Converter(quantidade, unidadeDe, produto, exata);
            }

            throw new ArgumentException("idUnidadePara ou idProduto precisa ser fornecido");
        }

        public static decimal Converter(decimal quantidade, UnidadeInformation unidadeDe, ProdutoInformation produto, bool exata = false)
        {
            if (produto == null || produto.IDProduto.HasValue == false)
                throw new ArgumentNullException();

            return Converter(quantidade, unidadeDe, produto.Unidade, exata);
        }

        public static decimal Converter(decimal quantidade, UnidadeInformation unidadeDe, UnidadeInformation unidadePara, bool exata = false)
        {
            if ((unidadeDe == null || unidadePara == null) || (unidadeDe.IDUnidade.HasValue == false || unidadePara.IDUnidade.HasValue == false))
                throw new ExceptionPDV(CodigoErro.E609);

            if (unidadeDe.IDUnidade.Value == unidadePara.IDUnidade.Value)
                return quantidade;

            if (quantidade == 0m)
                return 0;

            var conversoes = ListarConversoesDe(unidadeDe);
            var conversoesPara = ListarConversoesPara(unidadeDe);

            decimal fator = 0m;

            ConversaoUnidadeInformation conversaoDireta = conversoes.Where(c => c.Unidade_para.IDUnidade.Value == unidadePara.IDUnidade.Value).FirstOrDefault();
            ConversaoUnidadeInformation conversaoIndireta = null;

            if (conversaoDireta == null)
            {
                conversaoIndireta = conversoesPara.Where(c => c.Unidade_de.IDUnidade.Value == unidadePara.IDUnidade.Value).FirstOrDefault();
                if (conversaoIndireta == null)
                    throw new ExceptionPDV(CodigoErro.E60B, $"de {unidadeDe.Nome} para {unidadePara.Nome}");
                fator = conversaoIndireta.Divisao.Value;
            }
            else
                fator = conversaoDireta.Multiplicacao.Value;

            if (fator == 0)
                throw new ExceptionPDV(CodigoErro.E60D, $"de {unidadeDe.Nome} para {unidadePara.Nome}");

            var convertido = decimal.Round(quantidade / fator, 2, MidpointRounding.AwayFromZero);
            if (exata)
                return convertido;
            else
                return (int)convertido;
        }

        private static List<ConversaoUnidadeInformation> ListarConversoesPara(UnidadeInformation unidade)
        {
            return Listar().Where(c => c.Unidade_para.IDUnidade == unidade.IDUnidade.Value).ToList();
        }

        public static List<ConversaoUnidadeInformation> ListarConversoesDe(UnidadeInformation unidade)
        {
            return Listar().Where(c => c.Unidade_de.IDUnidade == unidade.IDUnidade.Value).ToList();
        }

        class UnidadeEqualityComparer : IEqualityComparer<UnidadeInformation>
        {
            public bool Equals(UnidadeInformation x, UnidadeInformation y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(UnidadeInformation obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
