using a7D.PDV.BigData.Shared.Model;
using a7D.PDV.BigData.Shared.ValueObject;
using Refit;
using System;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Server.BigData
{
    public interface IBigDataAPI
    {
        // TODO: Substituir por autenticação incluindo a chave de hardware
        [Get("/api/upload/{chave}/sync")]
        Task<bdAlteracaoInfo> StatusSync([Query] string chave);

        [Post("/api/upload/{chave}/sync")]
        Task<int> EndSync([Query] string chave, [Body] DateTime data);

        [Post("/api/upload/{chave}/produto")]
        Task<int> UploadProduto([Query] string chave, [Body] bdProduto[] produtos);

        [Post("/api/upload/{chave}/estoque")]
        Task<int> UploadEstoque([Query] string chave, [Body] bdEstoque[] estoque);

        [Post("/api/upload/{chave}/cliente")]
        Task<int> UploadCliente([Query] string chave, [Body] bdCliente[] clientes);

        [Post("/api/upload/{chave}/tipopagamento")]
        Task<int> UploadTipoPagamento([Query] string chave, [Body] bdTipoPagamento[] tipoPagamentos);

        [Post("/api/upload/{chave}/usuario")]
        Task<int> UploadUsuario([Query] string chave, [Body] bdUsuario[] usuarios);

        [Post("/api/upload/{chave}/pedido")]
        Task<int> UploadPedido([Query] string chave, [Body] bdPedido[] pedidos);
    }
}
