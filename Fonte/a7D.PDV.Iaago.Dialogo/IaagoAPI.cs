using a7D.PDV.Iaago.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoAPI
    {
        private static readonly Regex erForm = new Regex(@"(\w+)\s*=\s*(.*)$", RegexOptions.Multiline);

        public string url { get; set; }

        public string[] post { get; set; }

        public async Task Call(IaagoVars userIaago)
        {
            if (url == null)
            {
                return;
            }

            var curl = url;
            if (curl.Contains("@"))
            {
                curl = userIaago.PreencheMensagem(curl);
            }

            JToken result;
            using (var ws = new ClienteWS(curl))
            {
                if (post == null)
                {
                    result = await ws.Get<dynamic>(true);
                }
                else
                {
                    var formdata = new Dictionary<string, string>();
                    var m = erForm.Match(string.Join("\n", post));
                    while (m.Success)
                    {
                        if (m.Groups.Count == 3)
                        {
                            string key = m.Groups[1].Value.Trim();
                            string value = m.Groups[2].Value.Trim();
                            if (value == "''" || string.IsNullOrEmpty(value))
                            {
                                value = string.Empty;
                            }
                            else
                            {
                                value = userIaago.PreencheMensagem(value);
                            }
                            formdata.Add(key, value);
                        }

                        m = m.NextMatch();
                    }

                    result = await ws.Post<JToken>(formdata);
                }
            }

            userIaago.Add("@api", result.FromJTokenToDictionary(true), false);
        }
    }
}
