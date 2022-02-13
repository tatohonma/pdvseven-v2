using a7D.PDV.Integracao.API2.Model;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace a7D.PDV.Integracao.API2.Client
{
    public class TemaAPI
    {
        private ClienteWS api;

        public string Tema { get; set; }

        internal TemaAPI(ClienteWS ws)
        {
            api = ws;
        }

        public Tema CarregarTema(int idPDV)
            => api.Get<Tema>($"api/tema/{idPDV}", true);

        public MemoryStream Imagem(string idioma, string imagem)
        {
            var s = api.GetBytes($"imagensTema/{Tema}/{idioma}/{imagem}", true);
            if (s == null)
                return null;

            byte[] buffer = new byte[1024 * 1024];
            var ms = new MemoryStream();
            while (true)
            {
                int lido = s.Read(buffer, 0, buffer.Length);
                if (lido == 0)
                    break;
                else
                {
                    ms.Write(buffer, 0, lido);
                    Thread.Sleep(10);
                }
            }
            return ms;
        }
    }
}