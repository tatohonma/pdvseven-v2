// using System.Net;
//
// namespace a7D.PDV.Ativacao.API.Controllers
// {
//     [ApiAuth]
//     public class TipoPDVsController : ApiController
//     {
//         private AtivacaoContext db = new AtivacaoContext();
//
//         // GET: api/TipoPDVs
//         public IQueryable<TipoPDV> GetTipoPDVs()
//         {
//             return db.TipoPDVs;
//         }
//
//         // GET: api/TipoPDVs/5
//         [ResponseType(typeof(TipoPDV))]
//         public IHttpActionResult GetTipoPDV(int id)
//         {
//             TipoPDV tipoPDV = db.TipoPDVs.Find(id);
//             if (tipoPDV == null)
//             {
//                 return NotFound();
//             }
//
//             return Ok(tipoPDV);
//         }
//
//         // PUT: api/TipoPDVs/5
//         [ResponseType(typeof(void))]
//         public IHttpActionResult PutTipoPDV(int id, TipoPDV tipoPDV)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return BadRequest(ModelState);
//             }
//
//             if (id != tipoPDV.IDTipoPDV)
//             {
//                 return BadRequest();
//             }
//
//             db.Entry(tipoPDV).State = EntityState.Modified;
//
//             try
//             {
//                 db.SaveChanges();
//             }
//             catch (DbUpdateConcurrencyException ex)
//             {
//                 if (!TipoPDVExists(id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
//                 }
//             }
//
//             return StatusCode(HttpStatusCode.NoContent);
//         }
//
//         // POST: api/TipoPDVs
//         [ResponseType(typeof(TipoPDV))]
//         public IHttpActionResult PostTipoPDV(TipoPDV tipoPDV)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return BadRequest(ModelState);
//             }
//
//             db.TipoPDVs.Add(tipoPDV);
//             db.SaveChanges();
//
//             return CreatedAtRoute("DefaultApi", new { id = tipoPDV.IDTipoPDV }, tipoPDV);
//         }
//
//         // DELETE: api/TipoPDVs/5
//         [ResponseType(typeof(TipoPDV))]
//         public IHttpActionResult DeleteTipoPDV(int id)
//         {
//             TipoPDV tipoPDV = db.TipoPDVs.Find(id);
//             if (tipoPDV == null)
//             {
//                 return NotFound();
//             }
//
//             db.TipoPDVs.Remove(tipoPDV);
//             db.SaveChanges();
//
//             return Ok(tipoPDV);
//         }
//
//         protected override void Dispose(bool disposing)
//         {
//             if (disposing)
//             {
//                 db.Dispose();
//             }
//             base.Dispose(disposing);
//         }
//
//         private bool TipoPDVExists(int id)
//         {
//             return db.TipoPDVs.Count(e => e.IDTipoPDV == id) > 0;
//         }
//     }
// }