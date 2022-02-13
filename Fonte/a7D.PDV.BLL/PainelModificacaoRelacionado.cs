using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class PainelModificacaoRelacionado
    {
        public static PainelModificacaoRelacionadoInformation Carregar(int id)
        {
            var obj = new PainelModificacaoRelacionadoInformation { IDPainelModificacaoRelacionado = id };
            return CRUD.Carregar(obj) as PainelModificacaoRelacionadoInformation;
        }

        public static PainelModificacaoRelacionadoInformation CarregarCompleto(int id)
        {
            var painelRelacionado = Carregar(id);
            painelRelacionado.PainelModificacao1 = PainelModificacao.Carregar(painelRelacionado.PainelModificacao1.IDPainelModificacao.Value);
            painelRelacionado.PainelModificacao2 = PainelModificacao.Carregar(painelRelacionado.PainelModificacao2.IDPainelModificacao.Value);
            return painelRelacionado;
        }

        public static List<PainelModificacaoRelacionadoInformation> Listar()
        {
            var obj = new PainelModificacaoRelacionadoInformation();
            var lista = CRUD.Listar(obj).Cast<PainelModificacaoRelacionadoInformation>().ToList();
            return lista;
        }
        public static List<PainelModificacaoRelacionadoInformation> ListarCompleto()
        {            
            var retorno = new List<PainelModificacaoRelacionadoInformation>();
            foreach (var item in Listar())
            {
                item.PainelModificacao1 = PainelModificacao.Carregar(item.PainelModificacao1.IDPainelModificacao.Value);
                item.PainelModificacao2 = PainelModificacao.Carregar(item.PainelModificacao2.IDPainelModificacao.Value);
                retorno.Add(item);                
            }

            return retorno;
        }

        public static List<PainelModificacaoRelacionadoInformation> ListarCompletoPorPainel1(int idPainel1)
        {
            var retorno = new List<PainelModificacaoRelacionadoInformation>();
            foreach (var item in Listar().Where(p=>p.PainelModificacao1.IDPainelModificacao == idPainel1))
            {
                item.PainelModificacao1 = PainelModificacao.Carregar(item.PainelModificacao1.IDPainelModificacao.Value);
                item.PainelModificacao2 = PainelModificacao.Carregar(item.PainelModificacao2.IDPainelModificacao.Value);
                retorno.Add(item);
            }

            return retorno;
        }

     

        public static void Salvar(PainelModificacaoRelacionadoInformation painelRelacionado)
        {
            CRUD.Salvar(painelRelacionado);
        }

        private static void Adicionar(PainelModificacaoRelacionadoInformation painelRelacionado)
        {
            CRUD.Adicionar(painelRelacionado);
        }

        public static void Excluir(PainelModificacaoRelacionadoInformation painelRelacionado)
        {
            CRUD.Excluir(painelRelacionado);
        }

        public  static void ExcluirPorPainel1(Int32 idPainelModificacao1)
        {
            PainelModificacaoRelacionadoInformation obj = new PainelModificacaoRelacionadoInformation();
            obj = new PainelModificacaoRelacionadoInformation { PainelModificacao1 = new PainelModificacaoInformation { IDPainelModificacao = idPainelModificacao1} };

            CRUD.Excluir(obj);
        }
    }
}
