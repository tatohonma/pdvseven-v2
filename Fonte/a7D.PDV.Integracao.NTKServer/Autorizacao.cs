using System;
using System.Collections.Generic;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using System.IO;
using System.Text;

namespace a7D.PDV.Integracao.NTKServer
{
    public static class Autorizacao
    {
        internal static void Save(List<PGWParam> results)
        {
            string aut1 = results.Find(p => p.type == PTIINF.AUTLOCREF)?.value;
            string aut2 = results.Find(p => p.type == PTIINF.AUTHCODE)?.value;

            string cFile = Path.Combine(IntegraNTK.BasePath, string.Format(@"POS\{0:yyyyMMdd-HHmmss}-{1}-{2}.AUT", DateTime.Now, aut1, aut2));
            var sb = new StringBuilder();
            foreach (var r in results)
            {
                sb.AppendLine($">{r.field} {r.type.ToString()}: {r.value}");
            }
            File.WriteAllText(cFile, sb.ToString());
        }
    }
}