using a7D.PDV.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.iFood.DAL
{
    public class Pedido
    {
        public List<PedidoInformation> ListarStatusAlterados()
        {
            String sql = @"
                SELECT ...
            ";

            return null;
        }

        public void Adicionar()
        {
            String sql = @"
                INSERT ...
            ";
        }

        public void AlterarStatus()
        {
            String sql = @"
                UPDATE ...
            ";
        }
    }
}
