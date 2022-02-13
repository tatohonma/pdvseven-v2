using System.Threading.Tasks;

namespace a7D.PDV.Balanca.Interfaces
{
    public interface IBalanca
    {
        string Porta { get; set; }
        Task<Dados> LerPesoAsync();
        //Dados LerPesoWs(string enderecoWs);
    }
}
