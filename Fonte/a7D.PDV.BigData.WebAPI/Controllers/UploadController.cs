using a7D.PDV.BigData.Shared.Model;
using a7D.PDV.BigData.Shared.ValueObject;
using a7D.PDV.BigData.WebAPI.Models;
using a7D.PDV.BigData.WebAPI.Services;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace a7D.PDV.BigData.WebAPI.Controllers
{
    // API usa 'Retrofit' com interface IBigDataAPI
    // TODO: Fazer autenticação
    public class UploadController : ApiController
    {
        BDContext context;

        public UploadController()
        {
            context = new BDContext();
        }

        protected override ExceptionResult InternalServerError(Exception ex)
        {
            return base.InternalServerError(this.ObterDadosErro(ex));
        }

        [Route("api/upload/{chave}/sync")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> StatusSync(string chave, [FromUri]string versao = null)
        {
            string cEntidade = null;
            try
            {
                if (string.IsNullOrEmpty(chave) || chave.Length != 12)
                    return BadRequest();

                var info = new bdAlteracaoInfo();

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null)
                {
                    entidade = new Entidade()
                    {
                        ChaveAtivacao = chave,
                        Estabelecimento = "?",
                        RazaoSocial = "?",
                        CNPJ = "?",
                        Versao = versao
                    };
                    context.Entidades.Add(entidade);
                    await context.SaveChangesAsync();
                }

                cEntidade = entidade.IDEntidade + ": " + chave + " - ";

                if (entidade.Estabelecimento == "?" || entidade.UltimoSincronismo < DateTime.Now.AddHours(-4))
                {
                    try
                    {
                        var cliente = await AtivacoesServices.ClientePorChave(entidade.ChaveAtivacao);
                        if (cliente == null)
                        {
                            info.Mensagem = "Cliente não encontrado na base de ativações";
                        }
                        else if (string.IsNullOrEmpty(cliente.CNPJ))
                        {
                            entidade.Estabelecimento = cliente.Estabelecimento;
                            info.Mensagem = "Cliente não contem CNPJ ou CPF no cadastro de ativações: " + cliente.Estabelecimento;
                        }
                        else if (entidade.CNPJ != cliente.CNPJ
                         || entidade.Estabelecimento != cliente.Estabelecimento
                         || entidade.RazaoSocial != cliente.RazaoSocial)
                        {
                            entidade.CNPJ = cliente.CNPJ;
                            entidade.Estabelecimento = cliente.Estabelecimento;
                            entidade.RazaoSocial = cliente.RazaoSocial;
                            info.Mensagem = $"OK, Informações de propriedade atualizadas: {cliente.CNPJ}: {cliente.Estabelecimento}";
                        }
                        else
                            info.Mensagem = "OK, Entidade já atualizada";

                        entidade.Versao = versao;
                        entidade.UltimoSincronismo = DateTime.Now;
                        context.Entry(entidade).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        info.Mensagem =
                            "Não foi possível consultador o ativador para obter informações do cliente\n"
                            + UtilServices.ConcaternarErro(ex);
                    }
                }
                else if (entidade.CNPJ == "?")
                {
                    info.Mensagem = "Aguardando cadastro do CNPJ/CPF";
                    info.UltimoSincronismo = entidade.UltimoSincronismo;
                }
                else
                {
                    cEntidade += entidade.Estabelecimento;

                    info.Mensagem = "OK";

                    info.UltimoSincronismo = entidade.UltimoSincronismo;

                    info.Produto = (await context.Produtos
                        .Where(e => e.IDEntidade == entidade.IDEntidade)?
                        .OrderByDescending(p => p.dtAlteracao)
                        .FirstOrDefaultAsync())?
                        .dtAlteracao;

                    info.TipoPagamento = (await context.TipoPagamento
                        .Where(e => e.IDEntidade == entidade.IDEntidade)?
                        .OrderByDescending(p => p.IDTipoPagamento)
                        .FirstOrDefaultAsync())?
                        .IDTipoPagamento;

                    info.Cliente = (await context.Clientes
                        .Where(e => e.IDEntidade == entidade.IDEntidade)?
                        .OrderByDescending(p => p.dtAlteracao)
                        .FirstOrDefaultAsync())?
                        .dtAlteracao;

                    info.Usuario = (await context.Usuarios
                        .Where(u => u.IDEntidade == entidade.IDEntidade)?
                        .OrderByDescending(u => u.dtAlteracao)
                        .FirstOrDefaultAsync())?
                        .dtAlteracao;

                    info.Pedido = (await context.Pedidos
                        .Where(e => e.IDEntidade == entidade.IDEntidade)?
                        .OrderByDescending(p => p.DtPedidoFechamento)
                        .FirstOrDefaultAsync())?
                        .DtPedidoFechamento;
                }

                return Json(info);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/sync")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> EndSync(string chave, [FromBody]DateTime data)
        {
            string cEntidade = null;
            try
            {
                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;

                entidade.UltimoSincronismo = data;
                context.Entry(entidade).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(0);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/produto")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadProduto(string chave, bdProduto[] produtos)
        {
            string cEntidade = null;
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null || produtos == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;

                int novos = 0;
                foreach (var produto in produtos)
                {
                    var prod = await context
                        .Produtos
                        .FirstOrDefaultAsync(p => p.IDEntidade == entidade.IDEntidade
                                               && p.IDProduto == produto.IDProduto);

                    bool incluir = prod == null;
                    if (incluir)
                    {
                        novos++;
                        prod = new Produto()
                        {
                            IDEntidade = entidade.IDEntidade,
                            IDProduto = produto.IDProduto
                        };
                    }

                    prod.UpdateWith(produto);

                    if (incluir)
                        context.Produtos.Add(prod);
                    else
                        context.Entry(prod).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                return Ok(novos);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/estoque")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadEstoque(string chave, bdEstoque[] produtos)
        {
            string cEntidade = null;
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null || produtos == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;

                int alterados = 0;
                foreach (var produto in produtos)
                {
                    var prod = await context
                        .Produtos
                        .FirstOrDefaultAsync(p => p.IDEntidade == entidade.IDEntidade
                                               && p.IDProduto == produto.IDProduto);

                    if (prod == null) continue;

                    if (prod.EstoqueAtual != produto.EstoqueAtual)
                    {
                        alterados++;
                        prod.EstoqueAtual = produto.EstoqueAtual;
                        context.Entry(prod).State = EntityState.Modified;
                    }
                }

                if (alterados > 0)
                {
                    await context.SaveChangesAsync();
                    return Ok(alterados);
                }
                else
                    return Ok(0);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/cliente")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadCliente(string chave, bdCliente[] clientes)
        {
            string cEntidade = null;
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null || clientes == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;

                int novos = 0;
                foreach (var cliente in clientes)
                {
                    var cli = await context
                        .Clientes
                        .FirstOrDefaultAsync(c => c.IDEntidade == entidade.IDEntidade
                                               && c.IDCliente == cliente.IDCliente);

                    bool incluir = cli == null;
                    if (incluir)
                    {
                        novos++;
                        cli = new Cliente()
                        {
                            IDEntidade = entidade.IDEntidade,
                            IDCliente = cliente.IDCliente
                        };
                    }

                    cli.UpdateWith(cliente);

                    if (incluir)
                        context.Clientes.Add(cli);
                    else
                        context.Entry(cli).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                return Ok(novos);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/usuario")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadUsuario(string chave, bdUsuario[] usuarios)
        {
            string cEntidade = null;
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null || usuarios == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;

                int novos = 0;
                foreach (var usuario in usuarios)
                {
                    var usr = await context
                        .Usuarios
                        .FirstOrDefaultAsync(u => u.IDEntidade == entidade.IDEntidade
                                               && u.IDUsuario == usuario.IDUsuario);

                    bool incluir = usr == null;
                    if (incluir)
                    {
                        novos++;
                        usr = new Usuario()
                        {
                            IDEntidade = entidade.IDEntidade,
                            IDUsuario = usuario.IDUsuario
                        };
                    }

                    usr.UpdateWith(usuario);

                    if (incluir)
                        context.Usuarios.Add(usr);
                    else
                        context.Entry(usr).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                // Inativa usuário excluido
                //var usuariosBD = context.Usuarios.Where(u => u.IDEntidade == entidade.IDEntidade);
                //foreach (var usr in usuariosBD)
                //{
                //    if (usuarios.FirstOrDefault(u => u.IDUsuario == usr.IDUsuario) == null)
                //    {
                //        usr.Ativo = false;
                //        context.Entry(usr).State = EntityState.Modified;
                //    }
                //}

                await context.SaveChangesAsync();

                return Ok(novos);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/tipopagamento")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadTipoPagamento(string chave, bdTipoPagamento[] tipopagamentos)
        {
            string cEntidade = null;
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null || tipopagamentos == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;
                int novos = 0;
                foreach (var tipo in tipopagamentos)
                {
                    var tp = await context
                        .TipoPagamento
                        .FirstOrDefaultAsync(t => t.IDEntidade == entidade.IDEntidade
                                               && t.IDTipoPagamento == tipo.IDTipoPagamento);

                    bool incluir = tp == null;
                    if (incluir)
                    {
                        novos++;
                        tp = new TipoPagamento()
                        {
                            IDEntidade = entidade.IDEntidade,
                            IDTipoPagamento = tipo.IDTipoPagamento
                        };
                    }

                    tp.UpdateWith(tipo);

                    if (incluir)
                        context.TipoPagamento.Add(tp);
                    else
                        context.Entry(tp).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                return Ok(novos);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                return InternalServerError(ex);
            }
        }

        [Route("api/upload/{chave}/pedido")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadPedido(string chave, bdPedido[] pedidos)
        {
            string cEntidade = null;
            string cPedido = null;
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var entidade = await context
                    .Entidades
                    .FirstOrDefaultAsync(e => e.ChaveAtivacao == chave);

                if (entidade == null || pedidos == null) return NotFound();

                cEntidade = entidade.IDEntidade + ": " + entidade.Estabelecimento;

                int novos = 0;

                foreach (var pedido in pedidos)
                {
                    var pedAtual = await context.Pedidos
                        .FirstOrDefaultAsync(p => p.IDEntidade == entidade.IDEntidade
                                          && p.IDPedido == pedido.IDPedido);

                    if (pedAtual != null)
                    {
                        if (pedAtual.ValorTotal == pedido.ValorTotal)
                        {
                            continue;
                        }
                        else
                        {
                            context.Pedidos.Remove(pedAtual);
                            await context.SaveChangesAsync();
                        }
                    }

                    cPedido = pedido.ToString();

                    var ped = new Pedido()
                    {
                        IDEntidade = entidade.IDEntidade,
                        IDPedido = pedido.IDPedido,
                        IDCliente = pedido.IDCliente,
                        DtPedidoFechamento = pedido.DtPedidoFechamento,
                        ValorTotal = pedido.ValorTotal,
                        ValorDesconto = pedido.ValorDesconto,
                        ValorEntrega = pedido.ValorEntrega
                    };

                    context.Pedidos.Add(ped);

                    context.PedidoPagamentos.AddRange(pedido.Pagamentos.Select(pag => new PedidoPagamento()
                    {
                        IDEntidade = entidade.IDEntidade,
                        IDPedido = ped.IDPedido,
                        IDPedidoPagamento = pag.IDPedidoPagamento,
                        IDTipoPagamento = pag.IDTipoPagamento,
                        Valor = pag.Valor
                    }));

                    context.PedidoProdutos.AddRange(pedido.Produtos.Select(pp => new PedidoProduto()
                    {
                        IDEntidade = entidade.IDEntidade,
                        IDPedidoProduto = pp.IDPedidoProduto,
                        IDPedido = ped.IDPedido,
                        IDProduto = pp.IDProduto,
                        IDPedidoProduto_pai = pp.IDPedidoProduto_pai,
                        ValorUnitario = pp.ValorUnitario,
                        Quantidade = pp.Quantidade,
                        DtInclusao = pp.DtInclusao
                    }));

                    novos++;
                }

                await context.SaveChangesAsync();

                return Ok(novos);
            }
            catch (Exception ex)
            {
                ex.Data.Add("entidade", cEntidade);
                ex.Data.Add("pedido", cPedido);
                return InternalServerError(ex);
            }
        }
    }
}