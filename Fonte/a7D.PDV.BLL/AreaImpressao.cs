using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;

namespace a7D.PDV.BLL
{
    public static class AreaImpressao
    {
        public static List<AreaImpressaoInformation> Listar()
        {
            List<Object> listaObj = CRUD.Listar(new AreaImpressaoInformation());
            List<AreaImpressaoInformation> lista = listaObj.ConvertAll(new Converter<Object, AreaImpressaoInformation>(AreaImpressaoInformation.ConverterObjeto));

            foreach (var item in lista)
                item.TipoAreaImpressao = TipoAreaImpressao.Carregar(item.TipoAreaImpressao.IDTipoAreaImpressao.Value);

            return lista;
        }

        public static List<AreaImpressaoInformation> ListarSomenteProducao()
        {
            return Listar().Where(a => a.TipoAreaImpressao.IDTipoAreaImpressao == 2).ToList();
        }

        public static AreaImpressaoInformation Carregar(Int32 idAreaImpressao)
        {
            AreaImpressaoInformation obj = new AreaImpressaoInformation { IDAreaImpressao = idAreaImpressao };
            obj = (AreaImpressaoInformation)CRUD.Carregar(obj);
            if (obj != null && obj.TipoAreaImpressao != null)
                obj.TipoAreaImpressao = TipoAreaImpressao.Carregar(obj.TipoAreaImpressao.IDTipoAreaImpressao.Value);
            return obj;
        }

        public static void Salvar(AreaImpressaoInformation obj)
        {
            if (obj.TipoAreaImpressao.IDTipoAreaImpressao == 1)
            {
                foreach (var area in Listar().Where(a => a.IDAreaImpressao.Value != obj.IDAreaImpressao && a.TipoAreaImpressao.IDTipoAreaImpressao == 1))
                {
                    area.TipoAreaImpressao = new TipoAreaImpressaoInformation { IDTipoAreaImpressao = 0 };
                    CRUD.Salvar(area);
                }
            }
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idAreaImpressao)
        {
            try
            {
                AreaImpressaoInformation obj = new AreaImpressaoInformation { IDAreaImpressao = idAreaImpressao };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        //public static AreaImpressaoInformation AreaImpressaoSAT()
        //{
        //    return AreaImpressaoDAL.AreaImpressaoSAT();
        //}

        //public static int QuantidadeAreaSAT(int? idExcluido)
        //{
        //    return AreaImpressaoDAL.QuantidadeAreaSAT(idExcluido);
        //}
    }
}
