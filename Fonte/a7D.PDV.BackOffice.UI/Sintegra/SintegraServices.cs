using a7D.PDV.Fiscal.NFCe;
using Impactro.Layout;
using System;
using System.Text;

namespace a7D.PDV.BackOffice.UI.Sintegra
{
    public class ItemNF
    {
        public string timeStamp { get; set; }
        public string chaveconsulta { get; set; }
        public string arquivoCFeSAT { get; set; }
    }

    public static class SintegraServices
    {
        public static string Gerar(DateTime dtMes)
        {
            var layout = new Layout();

            var dtFinal = new DateTime(dtMes.Year, dtMes.Month, DateTime.DaysInMonth(dtMes.Year, dtMes.Month));

            var r10 = new Reg<Reg10>();
            r10[Reg10.CNPJ] = NFeFacade.Config.NFCe_CNPJ;
            r10[Reg10.IE] = NFeFacade.Config.NFCe_IE;
            r10[Reg10.nomeContribuinte] = NFeFacade.Config.NFCe_RazaoScocial;
            r10[Reg10.municipio] = NFeFacade.Config.NFCe_Municipio;
            r10[Reg10.UF] = NFeFacade.Config.xNFCe_UF;
            r10[Reg10.FAX] = "0000000000";
            r10[Reg10.dtInicial] = new DateTime(dtMes.Year, dtMes.Month, 1);
            r10[Reg10.dtFinal] = dtFinal > DateTime.Now ? DateTime.Now : dtFinal;
            r10[Reg10.codFinalidadeArquivMag] = "0";
            r10[Reg10.codIDNaturezaOp] = "0";
            r10[Reg10.codIDEstruturaMag] = "0";
            layout.Add(r10);

            var r11 = new Reg<Reg11>();
            r11[Reg11.Logradouro] = NFeFacade.Config.NFCe_Logradouro;
            r11[Reg11.num] = NFeFacade.Config.NFCe_Numero;
            r11[Reg11.complemento] = NFeFacade.Config.NFCe_Complemento;
            r11[Reg11.bairro] = NFeFacade.Config.NFCe_Bairro;
            r11[Reg11.CEP] = NFeFacade.Config.NFCe_CEP;
            r11[Reg11.nomeContato] = NFeFacade.Config.NFCe_Contato;
            r11[Reg11.telefone] = NFeFacade.Config.NFCe_Telefone;
            layout.Add(r11);

            string sqlNF = @"SELECT 
    timeStamp, 
    chaveconsulta,
    arquivoCFeSAT
FROM tbRetornoSAT
WHERE EEEEE='06000' AND timeStamp LIKE '" + dtMes.ToString("yyyyMM") + "%'";
            var nfs = EF.Repositorio.Query<ItemNF>(sqlNF);

            decimal totalGeral = 0;
            foreach (var nf in nfs)
            {
                if (string.IsNullOrEmpty(nf.arquivoCFeSAT) || nf.arquivoCFeSAT.Length < 2000)
                    continue;

                string arquivo = Encoding.UTF8.GetString(Convert.FromBase64String(nf.arquivoCFeSAT));
                if (!arquivo.StartsWith("<nfeProc "))
                    continue;

                NFeFacade.NFeProcObterDadosXML(arquivo,
                    out int modelo, out int serie, out long nNF, out int CFOP, out DateTime dtEmissao,
                    out decimal valorTotal, out decimal baseCalcICMS, out decimal vlrICMS);

                var r501 = new Reg<Reg50>();
                r501[Reg50.CNPJ] = NFeFacade.Config.NFCe_CNPJ;
                r501[Reg50.IE] = NFeFacade.Config.NFCe_IE;
                r501[Reg50.dtEmissao] = dtEmissao;
                r501[Reg50.UF] = NFeFacade.Config.xNFCe_UF;
                r501[Reg50.modelo] = modelo;
                r501[Reg50.serieNotaFiscal] = serie;
                r501[Reg50.numNotaFiscal] = nNF;
                r501[Reg50.CFOP] = CFOP;
                r501[Reg50.emitente] = "1";
                r501[Reg50.valorTotal] = valorTotal;
                r501[Reg50.baseCalcICMS] = baseCalcICMS;
                r501[Reg50.vlrICMS] = vlrICMS;

                totalGeral += valorTotal;

                layout.Add(r501);
            }

            var r53 = new Reg<Reg53>();
            r53[Reg53.CNPJ] = NFeFacade.Config.NFCe_CNPJ;
            r53[Reg53.IE] = NFeFacade.Config.NFCe_Logradouro;
            r53[Reg53.dtEmissao] = DateTime.Now;
            r53[Reg53.UF] = NFeFacade.Config.xNFCe_UF;
            r53[Reg53.modelo] = "01";
            r53[Reg53.serie] = "036";
            r53[Reg53.num] = 123;

            var r90 = new Reg<Reg90>();
            r90[Reg90.CNPJ] = NFeFacade.Config.NFCe_CNPJ;
            r90[Reg90.IE] = NFeFacade.Config.NFCe_IE;
            r90[Reg90.totalGeral] = totalGeral;
            layout.Add(r90);

            return layout.Conteudo;
        }
    }
}