using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.Services;
using a7D.PDV.BLL.Utils;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.NFCe;
using a7D.PDV.Model;
using BarcodeLib;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace a7D.PDV.Fiscal.Services
{
    public static class CupomSATService
    {
        public static string NomeTaxaServico = "Serviço";
        const string fonte = "Lucida Console";
        static Font fNormal = new Font(fonte, 8);

        public static bool ImprimirCupomVenda(string arquivoCFeSAT, int idPedido, string modeloImpressora, out Exception mensagemErro)
        {
            var pedido = Pedido.Carregar(idPedido);
            return ImprimirCupomVenda(arquivoCFeSAT, pedido, modeloImpressora, out mensagemErro, out byte[] image, false, Constantes.TotalWidth);
        }

        public static bool ImprimirCupomVenda(int idPedido, out byte[] image, int width)
        {
            if (width < Constantes.TotalWidth)// Tamanho minimo e padrão
                width = Constantes.TotalWidth;

            var pedido = Pedido.Carregar(idPedido);
            if (pedido?.RetornoSAT_venda?.IDRetornoSAT > 0)
            {
                var retornoSAT = RetornoSAT.Carregar(pedido.RetornoSAT_venda.IDRetornoSAT.Value);
                return ImprimirCupomVenda(retornoSAT.arquivoCFeSAT, pedido, null, out Exception mensagemErro, out image, true, width);
            }
            else
            {
                image = null;
                return false;
            }
        }

        public static string ObterAssinaturaQRCODE(string arquivoCFeSAT)
        {
            string arquivo = Encoding.UTF8.GetString(Convert.FromBase64String(arquivoCFeSAT));
            XDocument xml = XDocument.Parse(arquivo, LoadOptions.None);

            string chaveConsulta = xml.Root.Descendants("infCFe")
                .Attributes("Id")
                .Select(p => p.Value)
                .FirstOrDefault();

            string dataEmissao = xml.Root.Descendants("ide")
                .Descendants("dEmi")
                .Select(p => p.Value)
                .FirstOrDefault();

            string horaEmissao = xml.Root.Descendants("ide")
                .Descendants("hEmi")
                .Select(p => p.Value)
                .FirstOrDefault();

            string CPFCNPJCliente = xml.Root.Descendants("dest")
                .Descendants("CPF")
                .Select(p => p.Value)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(CPFCNPJCliente))
                CPFCNPJCliente = xml.Root.Descendants("dest")
                    .Descendants("CNPJ")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string valorTotalAPagar = xml.Root.Descendants("total")
                .Descendants("vCFe")
                .Select(p => p.Value)
                .FirstOrDefault();

            string assinaturaQRCode = xml.Root.Descendants("ide")
                .Descendants("assinaturaQRCODE")
                .Select(p => p.Value)
                .FirstOrDefault();

            string dadosQRCODE = string.Format("{0}|{1}{2}|{3}|{4}|{5}",
                chaveConsulta.StartsWith("CFe", true, CultureInfo.InvariantCulture) ? chaveConsulta.Remove(0, 3) : chaveConsulta,
                dataEmissao,
                horaEmissao,
                valorTotalAPagar,
                CPFCNPJCliente,
                assinaturaQRCode);

            return dadosQRCODE;
        }

        public static bool ImprimirCupomVenda(string arquivoCFeSAT, PedidoInformation pedido, string modeloImpressora, out Exception mensagemErro, out byte[] image, bool makeImage, int totalWidth)
        {
            image = null;
            StringBuilder sbLogInfo = new StringBuilder();
            try
            {
                sbLogInfo.AppendLine("Codificando o retorno para UTF-8");
                string arquivo = Encoding.UTF8.GetString(Convert.FromBase64String(arquivoCFeSAT));

                //arquivoCFeSAT = @"PENGZT48aW5mQ0ZlIElkPSJDRmUzNTE2MDYwNzM1ODkxNjAwMDE2NzU5MDAwMTQ4Mzk1MDAzNTQ2MDA3MjAzMiIgdmVyc2FvPSIwLjA2IiB2ZXJzYW9EYWRvc0VudD0iMC4wNiIgdmVyc2FvU0I9IjAxMDAwMCI+PGlkZT48Y1VGPjM1PC9jVUY+PGNORj4wMDcyMDM8L2NORj48bW9kPjU5PC9tb2Q+PG5zZXJpZVNBVD4wMDAxNDgzOTU8L25zZXJpZVNBVD48bkNGZT4wMDM1NDY8L25DRmU+PGRFbWk+MjAxNjA2MTA8L2RFbWk+PGhFbWk+MTgzMzEyPC9oRW1pPjxjRFY+MjwvY0RWPjx0cEFtYj4xPC90cEFtYj48Q05QSj4yMzQ4NzQwMzAwMDEwMjwvQ05QSj48c2lnbkFDPk9BQ05ua2pXekxkczNlL0I5QUh5MndHbXZ6TThCU1I1VithQ0JyNmxIUnFBemUzdGpZWHJ6djJFWHVjNXFOWDVjMloxenhRdlUxT0Z3RDUybjVPZ2I5ZTNIU0NiZ21mRFpidmUvUnhqMFlsRnRUQ2h3UXV1bHRaQTRMZTJWdjNLS1ZUSmpsNExQR2pFUFJlQU1JVDZpYWJ4U3J1Q2ZVcWRtdDNlWVVUWFEwMGxiamJWeTR0Yjgxcmk0Z2xmVmU1T2svdjhETDlvNEJpTUdUVEM1Qks0MmdFU0h3cEhXelFzSEdrdFJvLzZKKzk5N21RMm1RV1BlS1l2dUxla1pMN0ZPSkd6ZlArMDJsSlFCanoxb3E1OHlZU3Jjbnh6L2Y0QW5sN245MXg0RTE5R25WMkRDaGkzczBHbkJhWWV4YUV6eUI3cVd1VmtuREZGa2poTlFHdklmUT09PC9zaWduQUM+PGFzc2luYXR1cmFRUkNPREU+Ymcza2cxdUdJYTIxeW91RkFBMzhpeVBNUWJoUXlPd2loZHVIUnNWczNXSnNFblhLN09qM1JkeXV6bVlua1BlenF4M3lUTkhPdEdXaEgwL04vU0oxeFBGRFJkZTlFcnAyNDN2RUlDWmszQVd4eVVoU3ZvMzU4T1hWWno0SnppRy9RaEcxMWxvdHN1MjZkV0hQQ29QV1BVY2djWGlVaUFMR2NMRmxMUG1Nbk03KzI4cTBjdC9oVEJCR2NVQXlaK0RkMW9qck5pZUVsWjBkUlZtYVVPRjZXSDdBQWFCUkNWcGp6SmtQRHdaUFNGUldydFlxYkxtMS93NVNFbC8ybmNoa242OFczQlZiQnRMcERZY2dBakk4N0dOYzRFZ1p1ejgrOWlQTzRzT0pQYVFjd2pCSFJrcThiQ3g4MWp5bW5sdmFjekFxZCt5OU1KdjFVZmZUR21rd2xnPT08L2Fzc2luYXR1cmFRUkNPREU+PG51bWVyb0NhaXhhPjIwNzwvbnVtZXJvQ2FpeGE+PC9pZGU+PGVtaXQ+PENOUEo+MDczNTg5MTYwMDAxNjc8L0NOUEo+PHhOb21lPkdBTEVURVJJQSBDQVBPTEVUSSBMVERBIE1FPC94Tm9tZT48ZW5kZXJFbWl0Pjx4TGdyPkFWRU5JREEgSU1QRVJBVFJJWiBMRU9QT0xESU5BPC94TGdyPjxucm8+NDQxPC9ucm8+PHhDcGw+QU5FWE8gNDMzPC94Q3BsPjx4QmFpcnJvPk5PVkEgUEVUUk9QT0xJUzwveEJhaXJybz48eE11bj5TQU8gQkVSTkFSRE8gRE8gQ0FNUE88L3hNdW4+PENFUD4wOTc3MDI3MjwvQ0VQPjwvZW5kZXJFbWl0PjxJRT42MzU1MTQ0ODgxMTM8L0lFPjxjUmVnVHJpYj4xPC9jUmVnVHJpYj48aW5kUmF0SVNTUU4+TjwvaW5kUmF0SVNTUU4+PC9lbWl0PjxkZXN0PjwvZGVzdD48ZGV0IG5JdGVtPSIxIj48cHJvZD48Y1Byb2Q+MTE2PC9jUHJvZD48eFByb2Q+QWd1YSBDLyBHw6FzPC94UHJvZD48TkNNPjIyMDIxMDAwPC9OQ00+PENGT1A+NTQwNTwvQ0ZPUD48dUNvbT5VbjwvdUNvbT48cUNvbT4xLjAwMDA8L3FDb20+PHZVbkNvbT40LjUwPC92VW5Db20+PHZQcm9kPjQuNTA8L3ZQcm9kPjxpbmRSZWdyYT5BPC9pbmRSZWdyYT48dkl0ZW0+NC45NTwvdkl0ZW0+PHZSYXRBY3I+MC40NTwvdlJhdEFjcj48L3Byb2Q+PGltcG9zdG8+PElDTVM+PElDTVNTTjEwMj48T3JpZz4wPC9PcmlnPjxDU09TTj4xMDI8L0NTT1NOPjwvSUNNU1NOMTAyPjwvSUNNUz48UElTPjxQSVNTTj48Q1NUPjQ5PC9DU1Q+PC9QSVNTTj48L1BJUz48Q09GSU5TPjxDT0ZJTlNTTj48Q1NUPjQ5PC9DU1Q+PC9DT0ZJTlNTTj48L0NPRklOUz48L2ltcG9zdG8+PC9kZXQ+PHRvdGFsPjxJQ01TVG90Pjx2SUNNUz4wLjAwPC92SUNNUz48dlByb2Q+NC41MDwvdlByb2Q+PHZEZXNjPjAuMDA8L3ZEZXNjPjx2UElTPjAuMDA8L3ZQSVM+PHZDT0ZJTlM+MC4wMDwvdkNPRklOUz48dlBJU1NUPjAuMDA8L3ZQSVNTVD48dkNPRklOU1NUPjAuMDA8L3ZDT0ZJTlNTVD48dk91dHJvPjAuMDA8L3ZPdXRybz48L0lDTVNUb3Q+PHZDRmU+NC45NTwvdkNGZT48RGVzY0FjckVudHI+PHZBY3Jlc1N1YnRvdD4wLjQ1PC92QWNyZXNTdWJ0b3Q+PC9EZXNjQWNyRW50cj48L3RvdGFsPjxwZ3RvPjxNUD48Y01QPjAxPC9jTVA+PHZNUD40Ljk1PC92TVA+PC9NUD48dlRyb2NvPjAuMDA8L3ZUcm9jbz48L3BndG8+PC9pbmZDRmU+PFNpZ25hdHVyZSB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyI+PFNpZ25lZEluZm8geG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMjAwMS9SRUMteG1sLWMxNG4tMjAwMTAzMTUiPjwvQ2Fub25pY2FsaXphdGlvbk1ldGhvZD48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxkc2lnLW1vcmUjcnNhLXNoYTI1NiI+PC9TaWduYXR1cmVNZXRob2Q+PFJlZmVyZW5jZSBVUkk9IiNDRmUzNTE2MDYwNzM1ODkxNjAwMDE2NzU5MDAwMTQ4Mzk1MDAzNTQ2MDA3MjAzMiI+PFRyYW5zZm9ybXM+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIj48L1RyYW5zZm9ybT48VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMjAwMS9SRUMteG1sLWMxNG4tMjAwMTAzMTUiPjwvVHJhbnNmb3JtPjwvVHJhbnNmb3Jtcz48RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2Ij48L0RpZ2VzdE1ldGhvZD48RGlnZXN0VmFsdWU+OTBkdDRhSXVoS0NrWGFYNVZFUm1XSENQZkMvMjlFeTUzUWZNaEVDV3U1OD08L0RpZ2VzdFZhbHVlPjwvUmVmZXJlbmNlPjwvU2lnbmVkSW5mbz48U2lnbmF0dXJlVmFsdWU+RnJZTEhmYXZRUWpsNGxGTXV1Nzg5bEhtcWQ5U2pNdnUvM3duUWpXQ2tYTjU4SExpQngwN2wvT01VMlhnU2ROTUZialhscHY1Z1lWdGppZFUzRUhtZEFjd0VoVDJPL2gycDBsRUE0ZWlwMzBlbGdyVm9qYTVtbGtoWUs0M1ZKM1NueDdDTWZZNVZ1ajJkRGM3ZFMrekhGTG1uK2Z0QWtiRTgzNER5eG9Ob1Y5dGUrQlhZTThBTDJyTnJERjdwMTlhcFZXdE1QSmw1bEcvNjBFZDdXb2ZoQkdienlQbWNaVE1pRytCWnF3Uy9ZVnMyR1NEcy9BVzFySnlCKzdwanEzcXpaMDg5RkwzRWJwRFZ0bXJmdHhnL2tyOVY4VWNyUTV5eFg4M0N2d0h1c0RsZFZCbkF2R0haQ3NCV1R1ZnZDakZzUlJITjhTMnNWL0cwZitGZllQUE1BPT08L1NpZ25hdHVyZVZhbHVlPjxLZXlJbmZvPjxYNTA5RGF0YT48WDUwOUNlcnRpZmljYXRlPk1JSUdmakNDQkdhZ0F3SUJBZ0lKQVNLZEx5QWRSeDdaTUEwR0NTcUdTSWIzRFFFQkN3VUFNRkV4TlRBekJnTlZCQW9UTEZObFkzSmxkR0Z5YVdFZ1pHRWdSbUY2Wlc1a1lTQmtieUJGYzNSaFpHOGdaR1VnVTJGdklGQmhkV3h2TVJnd0ZnWURWUVFERXc5QlF5QlRRVlFnVTBWR1FWb2dVMUF3SGhjTk1UWXdNekk1TVRjeU16SXdXaGNOTWpFd016STVNVGN5TXpJd1dqQ0J0ekVTTUJBR0ExVUVCUk1KTURBd01UUTRNemsxTVFzd0NRWURWUVFHRXdKQ1VqRVNNQkFHQTFVRUNCTUpVMkZ2SUZCaGRXeHZNUkV3RHdZRFZRUUtFd2hUUlVaQldpMVRVREVQTUEwR0ExVUVDeE1HUVVNdFUwRlVNU2d3SmdZRFZRUUxFeDlCZFhSbGJuUnBZMkZrYnlCd2IzSWdRVklnVTBWR1FWb2dVMUFnVTBGVU1USXdNQVlEVlFRREV5bEhRVXhGVkVWU1NVRWdRMEZRVDB4RlZFa2dURlJFUVNCTlJUb3dOek0xT0RreE5qQXdNREUyTnpDQ0FTSXdEUVlKS29aSWh2Y05BUUVCQlFBRGdnRVBBRENDQVFvQ2dnRUJBSUR6TGZnM2EyaU9EVkNIL0tMUmJUbHE2bUJhMll4NHVVd3JsYXdhb0NpeVRmT0hNYm5CaGx4cisxSCtpaktSYWVUUWRwWm5ZdXJUZEhMYTlXVUcvaWhsWHNlcS9HNmg4THZmcWpJcFB2L1hSR0tMSHhTTEVjN283VGlndG4vSDZjOWhVV1dCWUxsNkxuTWoxYlk5V3Z0cXBqYTFIUW9HQWpGcmdMbGZ0SWhUTEpGbW1KblBFRmh0NXlkd2JvbmUzYUxiRC94bTNNN1JkTHQzSVNKS3lYUlFrV0pTdXU0ZkZHNUpzcHFqUjNZQVNhYmh6M1JPeGVnZW9MZWM0TnFTYmFHeFVVOGhKSkt2U1M3MnlZVG9XcDJGM210b1E4R1p2ZEtKMXMvNTgybXZJakRseHprcHZodmppMnNVU0UzcDlHaWp4TXpYTnBIQTczSHo2Mjh5dEdVQ0F3RUFBYU9DQWZBd2dnSHNNQTRHQTFVZER3RUIvd1FFQXdJRjREQjFCZ05WSFNBRWJqQnNNR29HQ1NzR0FRUUJnZXd0QXpCZE1Gc0dDQ3NHQVFVRkJ3SUJGazlvZEhSd09pOHZZV056WVhRdWFXMXdjbVZ1YzJGdlptbGphV0ZzTG1OdmJTNWljaTl5WlhCdmMybDBiM0pwYnk5a2NHTXZZV056WldaaGVuTndMMlJ3WTE5aFkzTmxabUY2YzNBdWNHUm1NR1VHQTFVZEh3UmVNRnd3V3FCWW9GYUdWR2gwZEhBNkx5OWhZM05oZEM1cGJYQnlaVzV6WVc5bWFXTnBZV3d1WTI5dExtSnlMM0psY0c5emFYUnZjbWx2TDJ4amNpOWhZM05oZEhObFptRjZjM0F2WVdOellYUnpaV1poZW5Od1kzSnNMbU55YkRDQmxBWUlLd1lCQlFVSEFRRUVnWWN3Z1lRd0xnWUlLd1lCQlFVSE1BR0dJbWgwZEhBNkx5OXZZM053TG1sdGNISmxibk5oYjJacFkybGhiQzVqYjIwdVluSXdVZ1lJS3dZQkJRVUhNQUtHUm1oMGRIQTZMeTloWTNOaGRDNXBiWEJ5Wlc1ellXOW1hV05wWVd3dVkyOXRMbUp5TDNKbGNHOXphWFJ2Y21sdkwyTmxjblJwWm1sallXUnZjeTloWTNOaGRDNXdOMk13RXdZRFZSMGxCQXd3Q2dZSUt3WUJCUVVIQXdJd0NRWURWUjBUQkFJd0FEQWtCZ05WSFJFRUhUQWJvQmtHQldCTUFRTURvQkFFRGpBM016VTRPVEUyTURBd01UWTNNQjhHQTFVZEl3UVlNQmFBRkxDRmdiTW96WFlxS1NSTFgxaEtiMmpackxBcU1BMEdDU3FHU0liM0RRRUJDd1VBQTRJQ0FRQXZwSjhGSHFoOEFFWXRHUzhQSWdJUjZrMlF3TTVIUDlCV2dUU2V5UjIrOHY5RDRNZDBkamZuckhqSlVHbFVDOUhhTTFOa2ZDbjMvQlhEWXJEamJzZVNLdFBqRWV3SzhFcjFyMFRaek1NSk9PSnpwL2srNVh2ZDlROGdiamo1TzhhV3dMaDUrL3NlczB4bDRKeUF4ekdkNHRSUGN1c2hQdUxpZXNXMzRrY2szTlRFajdoTGdYL3JEdzhMNWh4Nk10Ulg4Y2QzNUdBQzRTb2N1OUtQOFVubWkyR1FuaDVMeDFvK0pmcGJoSElNRTErVWsycWU3Kzd2VGl0RnZiUmdQYXhUMHdOcGQ3RnV3RWtRZWpRcGtXL1E5OFRQcEdvbjRQWGZQalZOUWdMc1JmdEVzdTJabU1lQmx6M2hqN3JLSjd4OGMySkJ3cjNoOGZPSjdYU1k4MjdRREFqS0t5WWR4dkcyTWZ3b2M2RGVENGphTUJmS0N5UytONzVFcE1Ec0dTbk4zQU9sbkthL3lYYnR3RU4ycHcxcUU2OFhiZmdaL2pjTlEzM1NTRnphc3BrSks2TEtrYnJUYUxUTUNra2FNRVVjc3drcXFMYTRaQWo4Wm91UTJIa0F2ZGIrRk43c090MVhaOHpJVVVzS0pzd0Y0Y3NyNFdRSnZVUkVDTFcvSER3Z2hEUUdnZGF1L0lyd01PeEM4eUttSjdTWUpFN3kwUjc1R0FMRHlCblJUZ0d2Vi8wSkNVd0JZK0JWR2RWS1NSWjRxc3NnQzZGaDhhdEcrSy9uWWdUZ0VYSHNJVGhVTGJRODN2QlZFbFhxWkxTWk82NUZGTnVuVEo2Q2R5Zk5TMnk1N2J6SmtpaFowQU1QT2wzMExFSEdEU2JEYXRTMzUrYURMa0F0RDBqSS9nPT08L1g1MDlDZXJ0aWZpY2F0ZT48L1g1MDlEYXRhPjwvS2V5SW5mbz48L1NpZ25hdHVyZT48L0NGZT4=";
                //arquivoCFeSAT = @"PENGZT48aW5mQ0ZlIElkPSJDRmUzNTE2MTIyMjk1ODcwMTAwMDE3MDU5MDAwMTE5OTM1MDAxMDUwODI4NjgyMiIgdmVyc2FvPSIwLjA3IiB2ZXJzYW9EYWRvc0VudD0iMC4wNyIgdmVyc2FvU0I9IjAxMDEwMCI+PGlkZT48Y1VGPjM1PC9jVUY+PGNORj44Mjg2ODI8L2NORj48bW9kPjU5PC9tb2Q+PG5zZXJpZVNBVD4wMDAxMTk5MzU8L25zZXJpZVNBVD48bkNGZT4wMDEwNTA8L25DRmU+PGRFbWk+MjAxNjEyMjc8L2RFbWk+PGhFbWk+MTYwMDU0PC9oRW1pPjxjRFY+MjwvY0RWPjx0cEFtYj4xPC90cEFtYj48Q05QSj4yMzQ4NzQwMzAwMDEwMjwvQ05QSj48c2lnbkFDPnBlVXlibTMyNWtoM1ZkMFpDZWdYWXlObnZ1bDdQbU11enA0U09zcTFlbHR5ODN0eTBwZ1NZa0t0VzJWbDFrb1JMWm1KRVdyeUVraDNGb21zL1Q0YlBqRFdPTmtFdjhDWjk0emhrQlF1WUhiTDRaS2Eyb1ovN3J1QmNoWDRtWURISlpZciswc3Mva0xUcHhDcUg2ZDVncm9xWW9sOFpOdmpaMVZpSzZmNlpaWDJlWHhqT3RrL1hMRTRoRHRaeDJndkY2TjBzK2JwZnRDOVJuYXFKTS8ybkdWUThBMkNrdDMxNWlSUDM4d2IzTU4rd1Q2aFpMQnNWUWVIVm1NMndHSzNKdXUxbWtCTVEvYTlCVXQ0UWlxUDR3LytKZlBJZHdXM0svTzdTR0U4UnBNZ3Npa284ZVlrdUc2bnUrQUZoemhkV0VlbG1FUUtOMFBWQnJ5VFFDZkh0dz09PC9zaWduQUM+PGFzc2luYXR1cmFRUkNPREU+ZE84RUJLZGI4Ung4YWM5dWN4N2RwUUVnaVBHZ29zOStUeWNZY2libm5sQkw0d1hxSVVKbmFteHYyb0svcDZlMElySTE1dkY4MkhLZ0xkdzZVWTdQWGFyOVBFVENKUE1DQ0dzSVBBK0t6a0VyN0w1WkR4aU9Wc0dZT01RV2s2c1lJU21TdXdoWS9mSHBFdVc1U3BGWFBoR0pVT1diWGRMZGFMNk04K3ZPUEk0TVZRaFJNVTZKNlhQNk9aSUZTWkFrU2RUNjhibEtsVlFGWkxia0syUzdaeEZCRWJTd1lkMjFUM2xscFo5NHpLRFJEUjJiN1pPN0NVaWJZVnRiZHhmRk11TlkwTkhLemEydU9pa0VJQzQwNTkrRk1NUHg2Z0FOV1FSYnR0QXJkKzBuMkdyZ0RPZWExVHhDK3QwaERYTDhlMjJpSS9aL0dJM3lEbkNyRkpPSjBBPT08L2Fzc2luYXR1cmFRUkNPREU+PG51bWVyb0NhaXhhPjQxMTwvbnVtZXJvQ2FpeGE+PC9pZGU+PGVtaXQ+PENOUEo+MjI5NTg3MDEwMDAxNzA8L0NOUEo+PHhOb21lPlNJUkkgR0FTVFJPIEJBUiBMVERBIE1FPC94Tm9tZT48eEZhbnQ+U0lSSSBCVVJHRVI8L3hGYW50PjxlbmRlckVtaXQ+PHhMZ3I+QVZFTklEQSBBTlRBUlRJQ0E8L3hMZ3I+PG5ybz40NTc8L25ybz48eEJhaXJybz5BR1VBIEJSQU5DQTwveEJhaXJybz48eE11bj5TQU8gUEFVTE88L3hNdW4+PENFUD4wNTAwMzAyMDwvQ0VQPjwvZW5kZXJFbWl0PjxJRT4xNDQ4NTE4NzkxMTg8L0lFPjxJTT41Mjk2NTM2ODwvSU0+PGNSZWdUcmliPjE8L2NSZWdUcmliPjxpbmRSYXRJU1NRTj5OPC9pbmRSYXRJU1NRTj48L2VtaXQ+PGRlc3Q+PC9kZXN0PjxkZXQgbkl0ZW09IjEiPjxwcm9kPjxjUHJvZD4yMzwvY1Byb2Q+PHhQcm9kPkJpZmUgQW5jaG8gR3JlbGhhZG88L3hQcm9kPjxOQ00+MjEwNjkwOTA8L05DTT48Q0ZPUD41MTAyPC9DRk9QPjx1Q29tPlVuPC91Q29tPjxxQ29tPjEuMDAwMDwvcUNvbT48dlVuQ29tPjk5LjkwPC92VW5Db20+PHZQcm9kPjk5LjkwPC92UHJvZD48aW5kUmVncmE+QTwvaW5kUmVncmE+PHZJdGVtPjAuMDE8L3ZJdGVtPjx2UmF0RGVzYz45OS44OTwvdlJhdERlc2M+PC9wcm9kPjxpbXBvc3RvPjx2SXRlbTEyNzQxPjQuMDA8L3ZJdGVtMTI3NDE+PElDTVM+PElDTVNTTjEwMj48T3JpZz4wPC9PcmlnPjxDU09TTj41MDA8L0NTT1NOPjwvSUNNU1NOMTAyPjwvSUNNUz48UElTPjxQSVNTTj48Q1NUPjQ5PC9DU1Q+PC9QSVNTTj48L1BJUz48Q09GSU5TPjxDT0ZJTlNTTj48Q1NUPjQ5PC9DU1Q+PC9DT0ZJTlNTTj48L0NPRklOUz48L2ltcG9zdG8+PC9kZXQ+PHRvdGFsPjxJQ01TVG90Pjx2SUNNUz4wLjAwPC92SUNNUz48dlByb2Q+OTkuOTA8L3ZQcm9kPjx2RGVzYz4wLjAwPC92RGVzYz48dlBJUz4wLjAwPC92UElTPjx2Q09GSU5TPjAuMDA8L3ZDT0ZJTlM+PHZQSVNTVD4wLjAwPC92UElTU1Q+PHZDT0ZJTlNTVD4wLjAwPC92Q09GSU5TU1Q+PHZPdXRybz4wLjAwPC92T3V0cm8+PC9JQ01TVG90Pjx2Q0ZlPjAuMDE8L3ZDRmU+PERlc2NBY3JFbnRyPjx2RGVzY1N1YnRvdD45OS44OTwvdkRlc2NTdWJ0b3Q+PC9EZXNjQWNyRW50cj48L3RvdGFsPjxwZ3RvPjxNUD48Y01QPjAxPC9jTVA+PHZNUD4wLjAxPC92TVA+PC9NUD48dlRyb2NvPjAuMDA8L3ZUcm9jbz48L3BndG8+PC9pbmZDRmU+PFNpZ25hdHVyZSB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyI+PFNpZ25lZEluZm8+PENhbm9uaWNhbGl6YXRpb25NZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAxL1JFQy14bWwtYzE0bi0yMDAxMDMxNSI+PC9DYW5vbmljYWxpemF0aW9uTWV0aG9kPjxTaWduYXR1cmVNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2Ij48L1NpZ25hdHVyZU1ldGhvZD48UmVmZXJlbmNlIFVSST0iI0NGZTM1MTYxMjIyOTU4NzAxMDAwMTcwNTkwMDAxMTk5MzUwMDEwNTA4Mjg2ODIyIj48VHJhbnNmb3Jtcz48VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI2VudmVsb3BlZC1zaWduYXR1cmUiPjwvVHJhbnNmb3JtPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAxL1JFQy14bWwtYzE0bi0yMDAxMDMxNSI+PC9UcmFuc2Zvcm0+PC9UcmFuc2Zvcm1zPjxEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGVuYyNzaGEyNTYiPjwvRGlnZXN0TWV0aG9kPjxEaWdlc3RWYWx1ZT5ORlBya2lXZy9pYkc2WmpvYnhienlzdGdwNWFXYVN0cXpydmFxUXIxVThRPTwvRGlnZXN0VmFsdWU+PC9SZWZlcmVuY2U+PC9TaWduZWRJbmZvPjxTaWduYXR1cmVWYWx1ZT5UKzZoWWhkUVFmVXYvRExIQmZGVHZWWm9ETzRmTnZZd0Y5M21FUDhON3YxRXpuRWVDUjNXMWdjWTJFUC9DdzdvNVliTFExa1FZbXBYRzFkNnJBN1BXVGF1T0E5SmpXT3dwZmp5T0ZvM1ZMc0FlRjN4b2M4dldBS0tGNmlIeTlrR0tUdlM1QVc0bnlGRVFRS2kzQ0p5UUJ4a0o0aUVnaVhFQm5zb05XQTVCeDl4VjVrelRFRVNRT0JFTlU1YldlUDREdXdaSW9YYW85b1I1N0daYzI2TGtQUXIxcEZPOWgwRVlwY0pxNzFlZHc1eFVSbVRHOHhDV0tPdFJhVnZlbUFFbnh3VkRzWGJOcURRYnNQZmhDcUNGSFpRSU9YbGdEMEZEUmdlMHBHRUJxcFNVZEtzNUh2NVgxSU9SdTFVc3JiZ0ZvWithdkY2aHVhRVN3YW1VdjR3MlE9PTwvU2lnbmF0dXJlVmFsdWU+PEtleUluZm8+PFg1MDlEYXRhPjxYNTA5Q2VydGlmaWNhdGU+TUlJR2V6Q0NCR09nQXdJQkFnSUpBUnowZk9SMU52Y0JNQTBHQ1NxR1NJYjNEUUVCQ3dVQU1GRXhOVEF6QmdOVkJBb1RMRk5sWTNKbGRHRnlhV0VnWkdFZ1JtRjZaVzVrWVNCa2J5QkZjM1JoWkc4Z1pHVWdVMkZ2SUZCaGRXeHZNUmd3RmdZRFZRUURFdzlCUXlCVFFWUWdVMFZHUVZvZ1UxQXdIaGNOTVRZd09URTFNVGd5TmpRM1doY05NakV3T1RFMU1UZ3lOalEzV2pDQnRERVNNQkFHQTFVRUJSTUpNREF3TVRFNU9UTTFNUXN3Q1FZRFZRUUdFd0pDVWpFU01CQUdBMVVFQ0JNSlUyRnZJRkJoZFd4dk1SRXdEd1lEVlFRS0V3aFRSVVpCV2kxVFVERVBNQTBHQTFVRUN4TUdRVU10VTBGVU1TZ3dKZ1lEVlFRTEV4OUJkWFJsYm5ScFkyRmtieUJ3YjNJZ1FWSWdVMFZHUVZvZ1UxQWdVMEZVTVM4d0xRWURWUVFERXlaVFNWSkpJRWRCVTFSU1R5QkNRVklnVEZSRVFTQk5SVG95TWprMU9EY3dNVEF3TURFM01EQ0NBU0l3RFFZSktvWklodmNOQVFFQkJRQURnZ0VQQURDQ0FRb0NnZ0VCQUlHdnQ2NVpLMHZrdmlQU0h2cHpvWHdJb3gzS3VOK3NhSVJLSE9SdndYZ3NQQkU3ZkYraldMdkJNTFhTT3U5anJrYmcrdkpLWkMxS1hBVllBa2lncVkrUTdTN3poRzZKbjZyR3ZUUlNscUtwOG95bjZaSHdOTFZXaTVvS3BSUXFWM0g5cWp3S1BtQm5URy8xQXNoWXcvbERYYVpQYUo5Y3h2THoxcWFaMDNLb2Z6UVUrRnNZYVpabGFZTGRWMExTaVd5TTlwd3lmelN3d1YyMUIrNkZaaUo2SmthbVo5S2JLVE16dU9PMSs4WXBFZkZtZzUxQkgyWUF5T1ZJVGs5Y0liOHJic2NHZi9jcUc0VWgvMmk2S2Y0RkRPUDdlYmdncXhYYTRCQ3JPdE5UbGpTU0JOVk9wWnZOSDd5eFhUREpWcWlyRitEMUI1THMxeDR3Tk5XbE1IVUNBd0VBQWFPQ0FmQXdnZ0hzTUE0R0ExVWREd0VCL3dRRUF3SUY0REIxQmdOVkhTQUViakJzTUdvR0NTc0dBUVFCZ2V3dEF6QmRNRnNHQ0NzR0FRVUZCd0lCRms5b2RIUndPaTh2WVdOellYUXVhVzF3Y21WdWMyRnZabWxqYVdGc0xtTnZiUzVpY2k5eVpYQnZjMmwwYjNKcGJ5OWtjR012WVdOelpXWmhlbk53TDJSd1kxOWhZM05sWm1GNmMzQXVjR1JtTUdVR0ExVWRId1JlTUZ3d1dxQllvRmFHVkdoMGRIQTZMeTloWTNOaGRDNXBiWEJ5Wlc1ellXOW1hV05wWVd3dVkyOXRMbUp5TDNKbGNHOXphWFJ2Y21sdkwyeGpjaTloWTNOaGRITmxabUY2YzNBdllXTnpZWFJ6WldaaGVuTndZM0pzTG1OeWJEQ0JsQVlJS3dZQkJRVUhBUUVFZ1ljd2dZUXdMZ1lJS3dZQkJRVUhNQUdHSW1oMGRIQTZMeTl2WTNOd0xtbHRjSEpsYm5OaGIyWnBZMmxoYkM1amIyMHVZbkl3VWdZSUt3WUJCUVVITUFLR1JtaDBkSEE2THk5aFkzTmhkQzVwYlhCeVpXNXpZVzltYVdOcFlXd3VZMjl0TG1KeUwzSmxjRzl6YVhSdmNtbHZMMk5sY25ScFptbGpZV1J2Y3k5aFkzTmhkQzV3TjJNd0V3WURWUjBsQkF3d0NnWUlLd1lCQlFVSEF3SXdDUVlEVlIwVEJBSXdBREFrQmdOVkhSRUVIVEFib0JrR0JXQk1BUU1Eb0JBRURqSXlPVFU0TnpBeE1EQXdNVGN3TUI4R0ExVWRJd1FZTUJhQUZMQ0ZnYk1velhZcUtTUkxYMWhLYjJqWnJMQXFNQTBHQ1NxR1NJYjNEUUVCQ3dVQUE0SUNBUUE2eHRwYm5id1VQRHJDV0ZGOUNzbHlJbXNTTkRnc2FkZjFKbVlRU1U4bjhYV0NIZ3ZIazF0TDQyemtVMTdXcTYwUFU2cThjTE5MVVYwOVg1WDZFd3gvc25NMzhDNHRMNjZUNHJVMkpxT1I3VzRFdzBmNDVaTHZiYzg2YWFxL2lMaGhNME1EY1ZzMjg0MEJmUW9VVmFBNFdwemQrZFdCWkU2OEtwNXRNc0lyTEt6eTdWZEZzVkQvTHpkbEsrcVhSaFBSNTkveE1hRngxWFRuSzMvZ05qOVV0aFMxdXVoUml0SFFjNWtXSG8wbTF5MlplbnJMUnoxZjU2ZkdHMStHRU41dHprdDdYYlJJbVpTLy9DSHhLWWVMaWNIa3NWdHpUM09WSktQQ25aeTdVU3BJMC93NXhxZDFVdHk0bGpvYTFRZGRZKzkwWTFQR2hDNGIzNUhoRExqeW5xUDVYODdoVGQ3OGd1a2tqc0NIWklDZHdlZnViS0orbExmOHBoWEt4bW1zbjc3K2x5ZWVrb1RhdkVKdjZza0kyelIzL0RSUzlhMDlncW5WRW9HdE1uQ0NBUlplZ1VibXJUZFhVVC9UQllIUndHeDJnd0RwZVc5a1BkNmx3YWUvUVEyUW0rNGlnRlRvSnlsTEFETXR2anI1L2w0TE94ZTVjcFdic2xZeDBKTG15YjRUZm5vTjhtdVF2ZjE2OWM1SVZzb3N0Q3NiS2dpaTdRRER4L3g3cTRVR29JaFBCTm1LWHBsMzNFWFdaS29sdVFSanFveGlTQ0x0aGpacTJ1SFFLZ2FaQ3ZYNk5PaXU5Uk1ackFpRFEyNGdtdlZHa0d4UVZPOUN0V2Nvejg1QW9mLzRvRUhUREM3M1kwOUwxUHdqREp3MCtyQ2I2U0JSRGlDekNwREFQdz09PC9YNTA5Q2VydGlmaWNhdGU+PC9YNTA5RGF0YT48L0tleUluZm8+PC9TaWduYXR1cmU+PC9DRmU+";
                //string fonte = "PDV7-Caixa|ImprimirCupomVenda";

                if (ConfiguracoesSistema.Valores.Fiscal == "NFCe") //arquivo.StartsWith("<nfeProc") || arquivo.StartsWith("<NFe"))
                {
                    return NFeFacade.Imprimir(arquivo, modeloImpressora, out mensagemErro, out image, makeImage, totalWidth);
                }

                sbLogInfo.AppendLine("Codificação concluída");
                sbLogInfo.AppendLine(arquivo);

                sbLogInfo.AppendLine("Recriando o arquivo XML");

                XDocument xml = XDocument.Parse(arquivo, LoadOptions.None);

                sbLogInfo.AppendLine("Arquivo XML Recriado");
                sbLogInfo.AppendLine(xml.ToString());
                sbLogInfo.AppendLine("Obtendo informações");

                string chaveConsulta = xml.Root.Descendants("infCFe").
                        Attributes("Id").
                        Select(p => p.Value).
                        FirstOrDefault();

                //if (ConfiguracoesSistema.Valores.SalvarXmlSat)
                //{
                //    var nomeXmlSat = (string.IsNullOrWhiteSpace(chaveConsulta) ? DateTime.Now.ToString("yyyyMMddHHmmss") : chaveConsulta) + ".xml";
                //    string caminho = Path.Combine(ConfiguracoesSistema.Valores.CaminhoXmlSat, nomeXmlSat);

                //    if (!Directory.Exists(ConfiguracoesSistema.Valores.CaminhoXmlSat))
                //        Directory.CreateDirectory(ConfiguracoesSistema.Valores.CaminhoXmlSat);

                //    xml.Save(caminho);
                //}

                string nomeFantasia = xml.Root.Descendants("emit").
                        Descendants("xFant").
                        Select(p => p.Value).
                        FirstOrDefault();

                string razaoSocial = xml.Root.Descendants("emit").
                        Descendants("xNome").
                        Select(p => p.Value).
                        FirstOrDefault();

                string rua = xml.Root.Descendants("emit").
                        Descendants("enderEmit").
                        Descendants("xLgr").
                        Select(p => p.Value).
                        FirstOrDefault();

                string numero = xml.Root.Descendants("emit").
                      Descendants("enderEmit").
                      Descendants("nro").
                      Select(p => p.Value).
                      FirstOrDefault();

                string bairro = xml.Root.Descendants("emit").
                        Descendants("enderEmit").
                        Descendants("xBairro").
                        Select(p => p.Value).
                        FirstOrDefault();

                string municipio = xml.Root.Descendants("emit").
                        Descendants("enderEmit").
                        Descendants("xMun").
                        Select(p => p.Value).
                        FirstOrDefault();

                string cep = xml.Root.Descendants("emit").
                        Descendants("enderEmit").
                        Descendants("CEP").
                        Select(p => p.Value).
                        FirstOrDefault();

                string endereco = string.Format("{0}, {1}" + Environment.NewLine + "{2}, {3}" + Environment.NewLine + "CEP:{4}" + Environment.NewLine, rua, numero, bairro, municipio, cep);

                string cnpj = xml.Root.Descendants("emit").
                        Descendants("CNPJ").
                        Select(p => p.Value).
                        LastOrDefault();

                string ie = xml.Root.Descendants("emit").
                        Descendants("IE").
                        Select(p => p.Value).
                        FirstOrDefault();

                string im = xml.Root.Descendants("emit").
                        Descendants("IM").
                        Select(p => p.Value).
                        FirstOrDefault();

                string numeroSAT = xml.Root.Descendants("ide").
                        Descendants("nserieSAT").
                        Select(p => p.Value).
                        FirstOrDefault();

                string dataEmissao = xml.Root.Descendants("ide").
                        Descendants("dEmi").
                        Select(p => p.Value).
                        FirstOrDefault();

                string horaEmissao = xml.Root.Descendants("ide").
                        Descendants("hEmi").
                        Select(p => p.Value).
                        FirstOrDefault();

                string numeroDocumento = xml.Root.Descendants("nCFe").
                        Select(p => p.Value).
                        FirstOrDefault();

                string CPFCNPJCliente = xml.Root.Descendants("dest")
                        .Descendants("CPF")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(CPFCNPJCliente))
                    CPFCNPJCliente = xml.Root.Descendants("dest")
                        .Descendants("CNPJ")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                string valorTotal = xml.Root.Descendants("total")
                        .Descendants("vProd")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                string valorDesconto = xml.Root.Descendants("total")
                        .Descendants("vDescSubtot")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                string valorAcrescimo = xml.Root.Descendants("total")
                        .Descendants("vAcresSubtot")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(valorAcrescimo))
                    valorAcrescimo = "0.00";

                if (string.IsNullOrWhiteSpace(valorDesconto))
                    valorDesconto = "0.00";

                string valorTotalAPagar = xml.Root.Descendants("total")
                        .Descendants("vCFe")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                string valorTroco = xml.Root.Descendants("vTroco")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                string assinaturaQRCode = xml.Root.Descendants("ide")
                        .Descendants("assinaturaQRCODE")
                        .Select(p => p.Value)
                        .FirstOrDefault();

                List<XElement> listaElementos = xml.Root.Descendants("det").ToList();

                string linha;
                string dadosItem;
                List<string> listaProduto = new List<string>();
                var listaProdutoValor = new List<ItemValor>();
                var listaPagamentoValor = new List<ItemValor>();
                Dictionary<string, decimal> listaNova = new Dictionary<string, decimal>();
                decimal impostosTotais = 0m;

                foreach (XElement elemento in listaElementos)
                {
                    var nItem = elemento.Attributes("nItem").Select(p => p.Value).FirstOrDefault();
                    List<XElement> produtos = elemento.Descendants("prod").ToList();
                    XElement imposto = elemento.Descendants("imposto").FirstOrDefault();

                    foreach (XElement elementoProduto in produtos)
                    {
                        var vProd = elementoProduto.Descendants("vProd").Select(p => p.Value).FirstOrDefault();
                        var xProd = elementoProduto.Descendants("xProd").Select(p => p.Value).FirstOrDefault();
                        var cProd = elementoProduto.Descendants("cProd").Select(p => p.Value).FirstOrDefault();
                        var uCom = elementoProduto.Descendants("uCom").Select(p => p.Value).FirstOrDefault();
                        var qCom = elementoProduto.Descendants("qCom").Select(p => p.Value).FirstOrDefault();
                        var vUnCom = elementoProduto.Descendants("vUnCom").Select(p => p.Value).FirstOrDefault();
                        var vRatDesc = elementoProduto.Descendants("vRatDesc").Select(p => p.Value).FirstOrDefault();
                        var vRatAcr = elementoProduto.Descendants("vRatAcr").Select(p => p.Value).FirstOrDefault();
                        var vItem = elementoProduto.Descendants("vItem").Select(p => p.Value).FirstOrDefault();

                        var vItem12741 = imposto.Descendants("vItem12741").Select(p => p.Value).FirstOrDefault();

                        dadosItem = "";
                        dadosItem += Convert.ToDecimal(qCom.Replace(".", ",")).ToString("#0.#") + uCom + "x";
                        dadosItem += Convert.ToDecimal(vUnCom.Replace(".", ",")).ToString("#0.00");

                        var vUcomDec = Convert.ToDecimal(vUnCom.Replace(".", ","));
                        var qComDec = Convert.ToDecimal(qCom.Replace(".", ","));

                        var impostosLinha = 0m;
                        if (string.IsNullOrWhiteSpace(vItem12741) == false)
                        {
                            if (decimal.TryParse(vItem12741.Replace(".", ","), out decimal dvItem12741))
                            {
                                impostosLinha = dvItem12741;
                                impostosTotais += impostosLinha;
                            }
                        }

                        var nLinha = $"{nItem} {xProd} {qComDec.ToString("#0.#")} {uCom} {vUcomDec.ToString("#0.00")} ({impostosLinha.ToString("#0.00")})";

                        listaNova.Add(nLinha, Convert.ToDecimal(vProd.Replace(".", ",")));

                        linha = "";
                        int limite = Constantes.Colunas - 12;
                        if (xProd.Length + dadosItem.Length > limite)
                            linha += Convert.ToInt32(nItem).ToString("000") + "|" + xProd.Substring(0, limite - dadosItem.Length) + "|" + dadosItem;
                        else
                            linha += (Convert.ToInt32(nItem).ToString("000") + "|" + xProd + "|" + dadosItem).PadRight(limite).Substring(0, limite);

                        linha += Convert.ToDecimal(vProd.Replace(".", ",")).ToString("#0.00").PadLeft(8);

                        listaProduto.Add(linha);

                    }
                }

                foreach (var item in listaNova)
                    listaProdutoValor.Add(new ItemValor(item.Key, item.Value, multLines: true));

                List<XElement> pagamentos = xml.Root.Descendants("MP").ToList();
                List<string> listaPagamentos = new List<string>();
                Dictionary<string, decimal> nListaPagamentos = new Dictionary<string, decimal>();
                string codigoPagamento = string.Empty;
                string valorPagamento = string.Empty;
                string linhaPagamento = string.Empty;
                MeioPagamentoSATInformation meioPagamento = null;
                foreach (XElement pagamento in pagamentos)
                {
                    codigoPagamento = pagamento.Descendants("cMP").Select(p => p.Value).FirstOrDefault();
                    valorPagamento = pagamento.Descendants("vMP").Select(p => p.Value).FirstOrDefault();

                    meioPagamento = MeioPagamentoSAT.CarregarPorCodigo(codigoPagamento);

                    linhaPagamento = "";
                    linhaPagamento += ((meioPagamento != null && meioPagamento.Descricao != null) ? meioPagamento.Descricao : codigoPagamento).PadRight(34).Substring(0, 34);
                    linhaPagamento += Convert.ToDecimal(valorPagamento.Replace(".", ",")).ToString("#0.00").PadLeft(9);
                    linhaPagamento += "\n";

                    var nLinha = $"{((meioPagamento != null && meioPagamento.Descricao != null) ? meioPagamento.Descricao : codigoPagamento)}";

                    if (nListaPagamentos.ContainsKey(nLinha))
                    {
                        var anterior = nListaPagamentos[nLinha];
                        nListaPagamentos[nLinha] = anterior += Convert.ToDecimal(valorPagamento.Replace(".", ","));
                    }
                    else
                    {
                        nListaPagamentos.Add(nLinha, Convert.ToDecimal(valorPagamento.Replace(".", ",")));
                    }

                    listaPagamentos.Add(linhaPagamento);
                }

                foreach (var item in nListaPagamentos)
                    listaPagamentoValor.Add(new ItemValor(item.Key, item.Value));

                nListaPagamentos.Add("Troco R$", Convert.ToDecimal(valorTroco.Replace(".", ",")));

                string dataHora = string.Format("{0}{1}", dataEmissao, horaEmissao);
                DateTime dtEmissao = DateTime.ParseExact(dataHora, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);

                //string logoCliente = ConverterImagemParaBase64(Properties.Resources.PDV7, System.Drawing.Imaging.ImageFormat.Jpeg);

                string dadosQRCODE = string.Format("{0}|{1}{2}|{3}|{4}|{5}", chaveConsulta.StartsWith("CFe", true, CultureInfo.InvariantCulture) ? chaveConsulta.Remove(0, 3) : chaveConsulta, dataEmissao, horaEmissao, valorTotalAPagar, CPFCNPJCliente, assinaturaQRCode);
                sbLogInfo.AppendLine("Obtendo QRCode");
                sbLogInfo.AppendLine("Dados QRCode\n" + dadosQRCODE);
                Bitmap bmpQrCode = GerarQRCODE(dadosQRCODE);

                string chaveConsultaSeparada;
                string chaveConsultaParaCodigoBarras = chaveConsulta.Replace("CFe", "");
                sbLogInfo.AppendLine("Obtendo Codigo de Barras");
                sbLogInfo.AppendLine("Dados Codigo de Barras\n" + chaveConsultaParaCodigoBarras);
                chaveConsultaSeparada = FormataChaveConsulta(chaveConsultaParaCodigoBarras);

                /*
                if (string.Equals("novo", tipoImpressaoSat, StringComparison.InvariantCultureIgnoreCase))
                {

                    using (var rv = new ReportViewer())
                    {
                        rv.Reset();
                        var lr = rv.LocalReport;
                        var cupom = cupomSat;

                        if (string.IsNullOrWhiteSpace(cupom) || !TiposCupom.Contains(cupom))
                            cupom = "elgin";

                        rv.Font = new Font("Arial MT Condensed", 10);
                        rv.ProcessingMode = ProcessingMode.Local;
                        lr.ReportEmbeddedResource = $"a7D.PDV.SAT.Cupom.SAT-{cupom}.rdlc";
                        lr.Refresh();
                        lr.DataSources.Clear();

                        var itemsDataSet = new DataTable("ItemsDataSet");
                        var subtotalDataSet = new DataTable("SubtotalDataSet");
                        var pagamentosDataSet = new DataTable("PagamentosDataSet");
                        var imagensDataSet = new DataTable("ImagensDataSet");

                        itemsDataSet.Columns.Add("Linha", typeof(string));
                        itemsDataSet.Columns.Add("Valor", typeof(decimal));

                        foreach (var produto in listaNova)
                        {
                            var row = itemsDataSet.NewRow();
                            row["Linha"] = produto.Key;
                            row["Valor"] = produto.Value;
                            itemsDataSet.Rows.Add(row);
                        }

                        subtotalDataSet.Columns.Add("Linha", typeof(string));
                        subtotalDataSet.Columns.Add("Valor", typeof(decimal));

                        var subtotalRow = subtotalDataSet.NewRow();
                        subtotalRow["Linha"] = "Subtotal";
                        subtotalRow["Valor"] = Convert.ToDecimal(valorTotal.Replace(".", ","));
                        subtotalDataSet.Rows.Add(subtotalRow);

                        var vvDesconto = Convert.ToDecimal(valorDesconto.Replace(".", ","));
                        if (vvDesconto > 0m)
                        {
                            subtotalRow = subtotalDataSet.NewRow();
                            subtotalRow["Linha"] = "Desconto R$";
                            subtotalRow["Valor"] = vvDesconto * -1;
                            subtotalDataSet.Rows.Add(subtotalRow);
                        }

                        var vvAcrescimo = Convert.ToDecimal(valorAcrescimo.Replace(".", ","));
                        if (vvAcrescimo > 0m)
                        {
                            subtotalRow = subtotalDataSet.NewRow();
                            subtotalRow["Linha"] = "Acréscimos R$";
                            subtotalRow["Valor"] = vvAcrescimo;
                            subtotalDataSet.Rows.Add(subtotalRow);
                        }

                        subtotalRow = subtotalDataSet.NewRow();
                        subtotalRow["Linha"] = "Total";
                        subtotalRow["Valor"] = Convert.ToDecimal(valorTotalAPagar.Replace(".", ","));
                        subtotalDataSet.Rows.Add(subtotalRow);

                        pagamentosDataSet.Columns.Add("Linha", typeof(string));
                        pagamentosDataSet.Columns.Add("Valor", typeof(decimal));

                        foreach (var pagamento in nListaPagamentos)
                        {
                            var row = pagamentosDataSet.NewRow();
                            row["Linha"] = pagamento.Key;
                            row["Valor"] = pagamento.Value;
                            pagamentosDataSet.Rows.Add(row);
                        }

                        imagensDataSet.Columns.Add("CodigoBarras", typeof(byte[]));
                        imagensDataSet.Columns.Add("QRCode", typeof(byte[]));

                        var qrCodeRow = imagensDataSet.NewRow();
                        var codigoBarras = GeraCodeBar(chaveConsultaParaCodigoBarras);
                        qrCodeRow["QRCode"] = (byte[])(new ImageConverter().ConvertTo(bmpQrCode, typeof(byte[])));
                        qrCodeRow["CodigoBarras"] = (byte[])(new ImageConverter().ConvertTo(codigoBarras, typeof(byte[])));

                        imagensDataSet.Rows.Add(qrCodeRow);

                        lr.DataSources.Add(new ReportDataSource("ItemsDataSet", itemsDataSet));
                        lr.DataSources.Add(new ReportDataSource("PagamentosDataSet", pagamentosDataSet));
                        lr.DataSources.Add(new ReportDataSource("SubtotalDataSet", subtotalDataSet));
                        lr.DataSources.Add(new ReportDataSource("ImagensDataSet", imagensDataSet));
                        sbLogInfo.AppendLine($"Estab: {razaoSocial}");

                        var isMesa = pedido.TipoPedido.IDTipoPedido == 10;
                        var numeroMesaComanda = "";

                        if (isMesa)
                        {
                            var mesa = Mesa.CarregarPorGUID(pedido.GUIDIdentificacao);
                            if (mesa != null)
                                numeroMesaComanda = Convert.ToString(mesa.Numero);
                        }
                        else
                        {
                            var comanda = Comanda.CarregarPorGUID(pedido.GUIDIdentificacao);
                            if (comanda != null)
                                numeroMesaComanda = Convert.ToString(comanda.Numero);
                        }

                        var nomeCliente = "";
                        if (pedido.Cliente != null)
                        {
                            pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);
                            nomeCliente = pedido.Cliente.NomeCompleto;
                        }

                        sbLogInfo.AppendLine($"EstabFantasia: {nomeFantasia}");
                        sbLogInfo.AppendLine($"Estab: {razaoSocial}");
                        sbLogInfo.AppendLine($"EnderecoEstab: {endereco}");
                        sbLogInfo.AppendLine($"CNPJ: {cnpj} IE: {ie} IM: {im}");
                        sbLogInfo.AppendLine($"CPFCliente: {CPFCNPJCliente}");
                        sbLogInfo.AppendLine($"CodigoBarras: {chaveConsultaParaCodigoBarras}");
                        sbLogInfo.AppendLine($"Pedido: {pedido.IDPedido.ToString()}");
                        sbLogInfo.AppendLine($"SatNo: {numeroSAT}");
                        sbLogInfo.AppendLine($"ExtratoNo: {numeroDocumento}");
                        sbLogInfo.AppendLine($"DataSat: {dtEmissao.ToString("dd/MM/yyyy - HH:mm:sss")}");
                        sbLogInfo.AppendLine($"Mesa: {Convert.ToString(isMesa)}");
                        sbLogInfo.AppendLine($"TributosTotais: {impostosTotais.ToString("#,##0.00")}");
                        sbLogInfo.AppendLine($"NumeroMesaComanda: {numeroMesaComanda}");
                        sbLogInfo.AppendLine($"NomeCliente: {nomeCliente}");

                        lr.SetParameters(new ReportParameter[] {
                            new ReportParameter("EstabFantasia", nomeFantasia),
                            new ReportParameter("Estab", razaoSocial),
                            new ReportParameter("EnderecoEstab", endereco),
                            new ReportParameter("CNPJIEIM", $"CNPJ {cnpj} IE {ie} IM {im}"),
                            new ReportParameter("CPFCliente", CPFCNPJCliente),
                            new ReportParameter("CodigoBarras", chaveConsultaParaCodigoBarras),
                            new ReportParameter("Pedido",pedido.IDPedido.ToString()),
                            new ReportParameter("SatNo", numeroSAT),
                            new ReportParameter("ExtratoNo", numeroDocumento),
                            new ReportParameter("DataSat", dtEmissao.ToString("dd/MM/yyyy - HH:mm:sss")),
                            new ReportParameter("Mesa", Convert.ToString(isMesa)),
                            new ReportParameter("TributosTotais", impostosTotais.ToString("#,##0.00")),
                            new ReportParameter("NumeroMesaComanda", numeroMesaComanda),
                            new ReportParameter("NomeCliente", nomeCliente)
                        });

                        if (makeImage)
                        {
                            image = rv.LocalReport.Render("Image",
                                "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "  <MarginTop>0in</MarginTop>" +
                                "  <MarginLeft>0in</MarginLeft>" +
                                "  <MarginRight>0in</MarginRight>" +
                                "  <MarginBottom>0in</MarginBottom>" +
                                "</DeviceInfo>",
                                out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);
                        }
                        else
                        {
                            using (var pd = new ReportPrintDocument(lr))
                            {
                                pd.PrinterSettings.PrinterName = modeloImpressora;
                                pd.Print();
                            }
                        }
                    }
                }
                else
                */
                {
                    var listBMP = GerarImagensCodigoBarras(chaveConsultaParaCodigoBarras, totalWidth);
                    var h = new ImpressaoHelper
                    {
                        NomeFantasia = nomeFantasia.Sanitizar(),
                        RazaoSocial = razaoSocial.Sanitizar(),
                        EnderecoCompleto = endereco,
                        CNPJ = cnpj,
                        InscricaoEstadual = ie,
                        InscricaoMunicipal = im,
                        NumeroSAT = numeroSAT,
                        DataEmissao = dtEmissao.ToString("dd/MM/yyyy HH:mm:ss"),
                        ChaveConsulta = chaveConsultaSeparada,
                        NumeroDocumento = numeroDocumento,
                        CodigoDeBarrasString = chaveConsultaSeparada,
                        CodigoBarras = listBMP,
                        QRCODE = bmpQrCode,
                        CPFCNPJCliente = CPFCNPJCliente,
                        Produtos = listaProduto.ToArray(),
                        ProdutosValores = listaProdutoValor.ToArray(),
                        Pagamentos = listaPagamentos.ToArray(),
                        PagamentosValores = listaPagamentoValor.ToArray(),
                        ValorTotal = Convert.ToDecimal(valorTotal.Replace(".", ",")).ToString("#0.00"),
                        IdPedido = pedido.IDPedido.ToString(),
                        ImpostosTotais = impostosTotais.ToString("N2")
                    };

                    var vDesconto = Convert.ToDecimal(valorDesconto.Replace(".", ","));
                    if (vDesconto > 0m)
                        h.ValorDesconto = (vDesconto * -1).ToString("#0.00");

                    var vAcrescimo = Convert.ToDecimal(valorAcrescimo.Replace(".", ","));
                    if (vAcrescimo > 0m)
                        h.ValorAcrescimo = vAcrescimo.ToString("#0.00");

                    h.ValorTotalAPagar = Convert.ToDecimal(valorTotalAPagar.Replace(".", ",")).ToString("#0.00");
                    h.ValorTroco = Convert.ToDecimal(valorTroco.Replace(".", ",")).ToString("#0.00");

                    sbLogInfo.AppendLine("Informações obtidas");

                    switch ((ETipoPedido)pedido.TipoPedido.IDTipoPedido.Value)
                    {
                        case ETipoPedido.Mesa:
                            h.Identificacao = $"MESA {Mesa.CarregarPorGUID(pedido.GUIDIdentificacao)?.Numero}";
                            break;
                        case ETipoPedido.Comanda:
                            h.Identificacao = $"COMANDA {Comanda.CarregarPorGUID(pedido.GUIDIdentificacao)?.Numero}";
                            break;
                        case ETipoPedido.Delivery:

                            if (pedido.OrigemPedido != null && pedido.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
                            {
                                TagInformation tagDisplayId = Tag.Carregar(pedido.GUIDIdentificacao, "ifood-DisplayID");
 
                                h.Identificacao = "IFOOD " + tagDisplayId.Valor;
                            }
                            else
                            {
                                h.Identificacao = "DELIVERY";
                            }

                            break;
                    }

                    if (makeImage)
                    {
                        sbLogInfo.AppendLine("Gerando Imagem!");

                        var bmp = new Bitmap(totalWidth, 10000);
                        var g = Graphics.FromImage(bmp);
                        int height = DesenhaFiscalSAT(g, h, totalWidth);
                        image = ImageUtil.ReduzEtransforma(bmp, height);
                        sbLogInfo.AppendLine($"Imagem com {image.Length} Bytes");
                    }
                    else
                    {
                        image = null;
                        sbLogInfo.AppendLine($"Enviando à impressora: {modeloImpressora}");

                        ImpressoraWindows.Imprimir(modeloImpressora, (s, a) => DesenhaFiscalSAT(a.Graphics, h, Constantes.TotalWidth));
                    }
                }
                mensagemErro = null;
                return true;
            }
            catch (Exception ex)
            {
                image = null;
                mensagemErro = new ExceptionPDV(CodigoErro.E504, ex);
                mensagemErro.Data.Add("arquivoCFeSAT", arquivoCFeSAT);
                mensagemErro.Data.Add("pedido", pedido.IDPedido);
                mensagemErro.Data.Add("modeloImpressora", modeloImpressora);
                //mensagemErro.Data.Add("tipoImpressaoSat", tipoImpressaoSat);
                //mensagemErro.Data.Add("cupomSat", cupomSat);
                mensagemErro.Data.Add("sbLogInfo", sbLogInfo.ToString());

                return false;
            }
        }

        public static Image GeraCodeBar(string dadosCodigoBarras, int width = 312, int height = 36)
        {
            return Barcode.DoEncode(TYPE.CODE128C, dadosCodigoBarras, false, Color.Black, Color.Transparent, width, height);
        }

        public static Bitmap GerarQRCODE(string dadosQRCODE)
        {
            var qrcodeEncoder = new QRCodeEncoder
            {
                CharacterSet = "UTF-8",
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L
            };
            return qrcodeEncoder.Encode(dadosQRCODE);
        }

        private static string FormataChaveConsulta(string chaveConsultaParaCodigoBarras)
        {
            var listaChaveConsulta = new List<string>();

            for (int i = 0; i < chaveConsultaParaCodigoBarras.Length; i += 4)
                listaChaveConsulta.Add(chaveConsultaParaCodigoBarras.Substring(i, 4) + "  ");

            string strAux = string.Empty;
            listaChaveConsulta.ForEach(p => strAux += p);
            return strAux.Trim();
        }

        private static List<Bitmap> GerarImagensCodigoBarras(string chaveConsultaParaCodigoBarras, int width)
        {
            var retorno = new List<Bitmap>();
            var height = 36;
            width -= 20;
            if (width > 300)
            {
                var img = Barcode.DoEncode(TYPE.CODE128C, chaveConsultaParaCodigoBarras, false, Color.Black, Color.Transparent, width, height);
                retorno.Add(new Bitmap(img));
            }
            else
            {
                int size = chaveConsultaParaCodigoBarras.Length / 2;

                var chave1 = chaveConsultaParaCodigoBarras.Substring(0, size);
                var img = Barcode.DoEncode(TYPE.CODE128C, chave1, false, Color.Black, Color.Transparent, width, height);
                retorno.Add(new Bitmap(img));

                var chave2 = chaveConsultaParaCodigoBarras.Substring(size + 1);
                img = Barcode.DoEncode(TYPE.CODE128C, chave2, false, Color.Black, Color.Transparent, width, height);
                retorno.Add(new Bitmap(img));
            }
            return retorno;
        }

        internal static int DesenhaFiscalSAT(Graphics g, ImpressaoHelper dados, int totalWidth)
        {
            ImpressoraWindows.ConfiguraFontes(g,
                out Font fTitulo,
                out Font fNormal,
                out Font fNormalB,
                out Font fPequena,
                out Font fPequenaB,
                totalWidth);

            Point p = new Point(0, 0);
            var img = ImageUtil.LogoPDV7_Horizontal_PB();

            p.X = totalWidth / 2 - img.Width / 2;
            g.DrawImage(img, p);

            var espaco = Constantes.Espacamento;
            p.Y += img.Height + espaco;

            if (!string.IsNullOrWhiteSpace(dados.NomeFantasia))
                p.Y += g.DrawCenter(fTitulo, dados.NomeFantasia, p.Y, totalWidth);
            else
                p.Y += g.DrawCenter(fTitulo, dados.RazaoSocial, p.Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.NomeFantasia))
                p.Y += g.DrawCenter(fPequena, dados.RazaoSocial, p.Y, totalWidth);

            p.Y += g.DrawCenter(fPequena, dados.EnderecoCompleto, p.Y, totalWidth);
            p.Y += g.DrawCenter(fPequena, $"CNPJ:{dados.CNPJ} IE:{dados.InscricaoEstadual} IM:{dados.InscricaoMunicipal}", p.Y, totalWidth);

            p.Y += g.DrawSeparador(p.Y, totalWidth);

            p.Y += g.DrawCenter(fTitulo, $"Extrato {dados.NumeroDocumento}", p.Y, totalWidth);
            p.Y += g.DrawCenter(fTitulo, "CUPOM FISCAL ELETRONICO - SAT", p.Y, totalWidth);

            p.Y += g.DrawSeparador(p.Y, totalWidth);

            p.Y += g.DrawText($"PEDIDO {dados.IdPedido}", dados.Identificacao, fPequena, p.Y, totalWidth);
            p.Y += g.DrawText($"CPF/CNPJ do Consumidor", dados.CPFCNPJCliente, fPequena, p.Y, totalWidth);

            p.Y += g.DrawSeparador(p.Y, totalWidth);

            g.DrawString("VL ITEM", fNormalB, Brushes.Black, new Rectangle(0, p.Y, totalWidth, 20), new StringFormat() { Alignment = StringAlignment.Far });
            p.Y += g.DrawText("# COD|DESC|QTD|VL UNIT", null, fNormalB, p.Y, totalWidth);

            foreach (var iv in dados.ProdutosValores)
                p.Y += g.DrawItemValor(iv, fNormal, p.Y, totalWidth);

            p.Y += espaco;
            p.Y += g.DrawItemValor(new ItemValor("Total:", dados.ValorTotal), fNormalB, p.Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.ValorDesconto))
                p.Y += g.DrawItemValor(new ItemValor("Desconto:", dados.ValorDesconto), fNormal, p.Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.ValorAcrescimo))
                p.Y += g.DrawItemValor(new ItemValor(NomeTaxaServico + ":", dados.ValorAcrescimo), fNormal, p.Y, totalWidth);

            p.Y += g.DrawItemValor(new ItemValor("Total a Pagar:", dados.ValorTotalAPagar), fNormal, p.Y, totalWidth);

            if (dados.PagamentosValores.Length > 0)
            {
                p.Y += espaco;
                foreach (var pagamento in dados.PagamentosValores)
                    p.Y += g.DrawItemValor(pagamento, fNormal, p.Y, totalWidth);
            }

            p.Y += espaco;
            p.Y += g.DrawItemValor(new ItemValor("Troco:", dados.ValorTroco), fNormal, p.Y, totalWidth);
            p.Y += g.DrawSeparador(p.Y, totalWidth);

            p.Y += g.DrawItemValor(new ItemValor("Tributos Totais (Lei Fed 12.741/12)", dados.ImpostosTotais) { ValorWidth = 50 }, fPequena, p.Y, totalWidth);
            p.Y += g.DrawText("* Valor aproximado dos tributos do item", null, fPequena, p.Y, totalWidth);
            p.Y += espaco;

            p.Y += g.DrawCenter(fNormalB, $"SAT No. {dados.NumeroSAT}", p.Y, totalWidth);
            p.Y += g.DrawCenter(fNormal, dados.DataEmissao, p.Y, totalWidth);
            p.Y += espaco;

            string part1 = dados.ChaveConsulta.Substring(0, 5 * 6);
            string part2 = dados.ChaveConsulta.Substring(5 * 6);
            p.Y += g.DrawCenter(fPequena, part1 + "\r\n" + part2, p.Y, totalWidth);

            p.X = (totalWidth - dados.CodigoBarras[0].Width) / 2;
            g.DrawImage(dados.CodigoBarras[0], p);
            p.Y += dados.CodigoBarras[0].Height + espaco;

            if (dados.CodigoBarras.Count == 2)
            {
                p.X = (totalWidth - dados.CodigoBarras[1].Width) / 2;
                g.DrawImage(dados.CodigoBarras[1], p);
                p.Y += dados.CodigoBarras[1].Height + espaco;
            }

            int maxSize = 200;
            if (dados.QRCODE.Width > maxSize)
            {
                p.X = (totalWidth - maxSize) / 2;
                g.DrawImage(dados.QRCODE, p.X, p.Y, maxSize, maxSize);
                p.Y += maxSize + espaco;
            }
            else
            {
                p.X = (totalWidth - dados.QRCODE.Width) / 2;
                g.DrawImage(dados.QRCODE, p);
                p.Y += dados.QRCODE.Height + espaco;
            }

            p.Y += g.DrawCenter(fNormal, "PDVSeven www.pdvseven.com.br", p.Y, totalWidth);
            return p.Y;
        }

    }
}
