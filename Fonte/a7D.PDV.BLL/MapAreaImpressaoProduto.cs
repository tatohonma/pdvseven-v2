using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Models;

namespace a7D.PDV.BLL
{
    public static class MapAreaImpressaoProduto
    {
        public static List<MapAreaImpressaoProdutoInformation> ListarPorAreaImpressao(Int32 idAreaImpressao)
        {
            MapAreaImpressaoProdutoInformation objFiltro = new MapAreaImpressaoProdutoInformation();
            objFiltro.AreaImpressao = new AreaImpressaoInformation { IDAreaImpressao = idAreaImpressao };

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<MapAreaImpressaoProdutoInformation> lista = listaObj.ConvertAll(new Converter<Object, MapAreaImpressaoProdutoInformation>(MapAreaImpressaoProdutoInformation.ConverterObjeto));

            foreach (var item in lista)
                item.Produto = Produto.Carregar(item.Produto.IDProduto.Value);

            return lista;
        }

        public static MapAreaImpressaoProdutoInformation Carregar(Int32 idMapAreaImpressaoProduto)
        {
            MapAreaImpressaoProdutoInformation obj = new MapAreaImpressaoProdutoInformation { IDMapAreaImpressaoProduto = idMapAreaImpressaoProduto };
            obj = (MapAreaImpressaoProdutoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(MapAreaImpressaoProdutoInformation obj)
        {
            CRUD.Salvar(obj);
        }
        public static void Excluir(Int32 idAreaImpressao, Int32 idProduto)
        {
            MapAreaImpressaoProdutoInformation objFiltro = new MapAreaImpressaoProdutoInformation();
            objFiltro.AreaImpressao = new AreaImpressaoInformation { IDAreaImpressao = idAreaImpressao };
            objFiltro.Produto = new ProdutoInformation { IDProduto = idProduto };

            CRUD.Excluir(objFiltro);
        }

        public static Boolean ProdutoMapeado(Int32 idAreaImpressao, Int32 idProduto)
        {
            MapAreaImpressaoProdutoInformation objFiltro = new MapAreaImpressaoProdutoInformation();
            objFiltro.AreaImpressao = new AreaImpressaoInformation { IDAreaImpressao = idAreaImpressao };
            objFiltro.Produto = new ProdutoInformation { IDProduto = idProduto };

            MapAreaImpressaoProdutoInformation obj = (MapAreaImpressaoProdutoInformation)CRUD.Carregar(objFiltro);

            return (obj.IDMapAreaImpressaoProduto != null);
        }

        public static List<AreaImpressaoInformation> AreasMapeadas(this List<MapAreaImpressaoProdutoInformation> mapeamento, ProdutoInformation produto)
        {
            return AreasMapeadas(mapeamento, produto.IDProduto.Value);
        }

        public static List<AreaImpressaoInformation> AreasMapeadas(this List<MapAreaImpressaoProdutoInformation> mapeamento, int idProduto)
        {
            return mapeamento.Where(mp => mp.Produto.IDProduto == idProduto).Select(mp => mp.AreaImpressao).ToList();
        }

        public static List<MapAreaImpressaoProdutoInformation> Listar()
        {
            return CRUD.Listar(new MapAreaImpressaoProdutoInformation()).Cast<MapAreaImpressaoProdutoInformation>().ToList();
        }

        public static List<MapAreaImpressaoProdutoInformation> ListarPorProduto(int? idProduto)
        {
            if (!idProduto.HasValue)
                return Enumerable.Empty<MapAreaImpressaoProdutoInformation>().ToList();
            var filtro = new MapAreaImpressaoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = idProduto.Value }
            };

            var result = CRUD.Listar(filtro).Cast<MapAreaImpressaoProdutoInformation>().ToList();

            foreach (var map in result)
            {
                map.AreaImpressao = AreaImpressao.Carregar(map.AreaImpressao.IDAreaImpressao.Value);
            }

            return result;
        }

        public static void Adicionar(MapAreaImpressaoProdutoInformation mapAreaImpressaoProduto)
        {
            CRUD.Adicionar(mapAreaImpressaoProduto);
        }

        public static tbAreaImpressao[] AreasProduto(int idProduto)
        {
            return EF.Repositorio.ListarConfig<tbMapAreaImpressaoProduto>(
                tb => tb.Include(nameof(tbOrdemImpressao.tbAreaImpressao)),
                a => a.IDProduto == idProduto)
                .Select(r => r.tbAreaImpressao)
                .ToArray();
        }
    }
}
