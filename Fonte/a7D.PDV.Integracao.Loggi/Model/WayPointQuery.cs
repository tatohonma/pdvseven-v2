using System.Text.RegularExpressions;

namespace a7D.PDV.Integracao.Loggi.Model
{
    public class WayPointQuery
    {
        public string by;
        public WayPoint query;

        public WayPointQuery()
        {
        }

        public WayPointQuery(string endreco)
        {
            endreco = endreco.Trim();
            query = new WayPoint();

            // https://regex101.com/
            var r = new Regex(@"((\d{5}-\d{3})|(\d{8}))\s*,*\s*(\d+)");
            var m = r.Match(endreco);
            if (m.Success)
            {
                by = "cep";
                query.cep = m.Groups[1].Value.Replace("-", "");
                if (m.Groups.Count > 3)
                    query.number = int.Parse(m.Groups[m.Groups.Count-1].Value);

                query.address_complement = endreco.Substring(m.Index + m.Length).Trim();
                if (string.IsNullOrEmpty(query.address_complement))
                    query.address_complement = null;

                return;
            }

            by = "address";
            int n = endreco.IndexOf(" ");
            if (n > 0)
            {
                query.category = endreco.Substring(0, n);

                if (query.category.ToLower() == "r")
                    query.category = "Rua";
                else if (query.category.ToLower() == "av")
                    query.category = "Avenida";

                endreco = endreco.Substring(n + 1);
                r = new Regex(@",\s*(\d+)");
                m = r.Match(endreco);
                if (m.Success)
                {
                    query.address = endreco.Substring(0, m.Index).Trim();
                    query.number = int.Parse(m.Groups[1].Value);
                    query.address_complement = endreco.Substring(m.Index + m.Length).Trim();
                    if (string.IsNullOrEmpty(query.address_complement))
                        query.address_complement = null;
                }
                else
                    query.address = endreco.Trim();
            }
            else
                query.address = endreco;
        }

        public WayPointQuery(string cep, int numero, string complemento = null)
        {
            by = "cep";
            query = new WayPoint(cep, numero, complemento);
        }

        public WayPointQuery(string categoria, string endereco, int numero, string complemento = null)
        {
            by = "address";
            query = new WayPoint(categoria, endereco, numero, complemento);
        }
    }
}