using a7D.PDV.Integracao.EasyChopp.Model;
using Refit;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.EasyChopp
{
    public interface IEasyChopp
    {
        [Get("/getCliente?key={key}&nrDocumento={nrDocumento}")]
        Task<Cliente> getClienteDocumento([Query] string key, [Query]string nrDocumento);

        //[Get("/getCliente?key={key}&idTAG={idTAG}")]
        //Task<Cliente> getClienteTAG([Query] string key, [Query]string idTAG);

        [Get("/getClientes?key={key}&dtCadastro={dtCadastro}")]
        Task<ClienteList> getClientes([Query] string key, string dtCadastro);

        [Multipart]
        [Post("/addCliente")]
        Task<Retorno> addClienteDocumento(string key, string nrDocumento, string dsNomeCliente, string dsEmail, string dtNascimento, string dsSexo);

        //[Multipart]
        //[Post("/addCliente")]
        //Task<Retorno> addClienteTAG(string key, string idTAG, string dsNomeCliente, string dsEmail, string dtNascimento, string dsSexo);

        [Get("/getCredito?key={key}&nrDocumento={nrDocumento}")]
        Task<Cliente> getCreditoDocumento([Query] string key, [Query]string nrDocumento);

        //[Get("/getCredito?key={key}&idTAG={idTAG}")]
        //Task<Cliente> getCreditoTAG([Query] string key, [Query]string idTAG);

        [Get("/addTAG?key={key}&nrDocumento={nrDocumento}&idTAG={idTAG}&idTipoCartao={idTipoCartao}&stCobrancaTarifa={stCobrancaTarifa}&dsEmailUsuario={dsEmailUsuario}")]
        Task<Retorno> addTAG([Query] string key, [Query]string nrDocumento, [Query]string idTAG, [Query]string idTipoCartao, [Query]string stCobrancaTarifa, [Query]string dsEmailUsuario);

        [Get("/removeTAG?key={key}&nrDocumento={nrDocumento}&idTAG={idTAG}")]
        Task<Retorno> removeTAG([Query] string key, [Query]string nrDocumento, [Query]string idTAG);

        [Multipart]
        [Post("/addCredito")]
        Task<Retorno> addCreditoDocumento(string key, string nrDocumento, string vlValorCredito, string codAutorizacao, string dsEmailUsuario, bool stSituacaoPagamento, int idFormaPagamento, string nrCupom, string dsObservacao);

        //[Multipart]
        //[Post("/addCredito")]
        //Task<Retorno> addCreditoTAG(string key, string idTAG, string vlValorCredito, string codAutorizacao, string dsEmailUsuario, bool stSituacaoPagamento, int idFormaPagamento, string nrCupom, string dsObservacao);
    }
}
