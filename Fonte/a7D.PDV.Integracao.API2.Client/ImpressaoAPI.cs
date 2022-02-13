using System;
using System.Drawing;
using System.IO;

namespace a7D.PDV.Integracao.API2.Client
{
    public class ImpressaoAPI
    {
        private ClienteWS api;

        internal ImpressaoAPI(ClienteWS ws)
            => api = ws;

        public Stream ImagemConta(int id, int width, int pessoas = 0)
           => api.GetBytes($"impressao/conta/{id}/width/{width}/{pessoas}", true);

        public string TextoConta(int id, int colunas, int pessoas = 0)
           => api.GetText($"impressao/conta/{id}/cols/{colunas}/{pessoas}", true);

        public string TextoFiscal(int id, int colunas)
           => api.GetText($"impressao/sat/{id}/cols/{colunas}", true);

        public Stream Ticket(int id, int item, string tipo = null)
           => api.GetBytes($"impressao/ticket/{id}/{item}/{tipo}", true);

        public Stream ImageFiscal(int id, int width)
           => api.GetBytes($"impressao/sat/{id}/width/{width}", true);
    }
}