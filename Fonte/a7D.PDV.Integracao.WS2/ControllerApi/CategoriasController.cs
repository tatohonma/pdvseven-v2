using a7D.PDV.EF.DAO;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class CategoriasController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetCategorias(
            [FromUri] string data = "0")
        {
            try
            {
                DateTime? dtAlteracao = null;
                if (!string.IsNullOrEmpty(data) && data != "0" && long.TryParse(data, out long valData))
                {
                    dtAlteracao = DateTimeOffset.FromUnixTimeSeconds(valData).ToLocalTime().DateTime;
                }

                var result = new List<Categoria>();
                var categorias = CategoriaDAO.Listar(dtAlteracao);

                foreach (var categoria in categorias)
                {
                   result.Add(DTO.Converter(categoria));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(new Erro(20, ex)));
            }
        }

        [HttpPost]
        public IHttpActionResult PostCategoria([FromBody] Categoria categoria)
        {
            try
            {
                
                 
                var novaCategoria = new CategoriaProdutoInformation
                {
                    Nome = categoria.Nome,
                    Disponibilidade = true,
                    DtAlteracaoDisponibilidade = DateTime.Now,
                    DtUltimaAlteracao = DateTime.Now,
                    Excluido = false,
                    
                };
                BLL.CategoriaProduto.Salvar(novaCategoria);
                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, ex)));
            }
        }
    }
}
