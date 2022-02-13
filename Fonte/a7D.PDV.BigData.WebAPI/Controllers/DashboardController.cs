using a7D.PDV.BigData.WebAPI.Models;
using a7D.PDV.BigData.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace a7D.PDV.BigData.WebAPI.Controllers
{
    public class DashboardController : ApiController
    {
        BDContext context;

        public DashboardController()
        {
            context = new BDContext();
        }

        protected override ExceptionResult InternalServerError(Exception ex)
        {
            return base.InternalServerError(UtilServices.ConcaternarErro(ex));
        }

        [Route("api/dashboard/{nome}")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> Export(string nome)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(await this.BadRequestModel());

                var q = context.Querys.FirstOrDefault(i => i.Nome == nome);
                if (q == null) return NotFound();

                var conn = ConfigurationManager.ConnectionStrings["connectionBDString"].ConnectionString;
                var cn = new SqlConnection(conn);
                var da = new SqlDataAdapter(q.Query, cn);
                var tb = new DataTable();
                da.Fill(tb);

                var results = new List<object[]>();
                foreach (DataRow row in tb.Rows)
                    results.Add(row.ItemArray);

                return Json(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
