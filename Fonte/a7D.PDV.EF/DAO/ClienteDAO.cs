using a7D.PDV.EF.Models;

namespace a7D.PDV.EF.DAO
{
    public static class ClienteDAO
    {
        public static tbCliente Carregar(int id)
        {
            return Repositorio.Carregar<tbCliente>(c => c.IDCliente == id);
        }
    }
}
