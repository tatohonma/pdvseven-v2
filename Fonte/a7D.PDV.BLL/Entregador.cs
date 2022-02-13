using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class Entregador
    {
        public static List<EntregadorInformation> Listar()
        {
            return CRUD.Listar(new EntregadorInformation { Excluido = false }).Cast<EntregadorInformation>().ToList();
        }

        public static List<EntregadorInformation> ListarAtivos()
        {
            return Listar().Where(e => e.Ativo == true).ToList();
        }

        public static EntregadorInformation Carregar(int idEntregador)
        {
            return CRUD.Carregar(new EntregadorInformation { IDEntregador = idEntregador }) as EntregadorInformation;
        }

        public static void Salvar(EntregadorInformation entregador)
        {
            CRUD.Salvar(entregador);
        }

        public static void Excluir(int idEntregador)
        {
            var entregador = Carregar(idEntregador);
            Excluir(entregador);
        }

        public static void Excluir(EntregadorInformation entregador)
        {
            if (entregador == null)
                return;
            entregador.Excluido = true;
            Salvar(entregador);
        }
    }
}
