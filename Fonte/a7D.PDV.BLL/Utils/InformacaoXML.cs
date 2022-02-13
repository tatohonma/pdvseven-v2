using a7D.PDV.BLL;
using System;
using System.Text;
using System.Xml.Linq;

namespace a7D.PDV.BLL.Utils
{
    public class InformacaoXML
    {
        public int Tipo { get; set; }
        public string NomeArquivo { get; set; }
        public string XMLString { get; set; }
        public XDocument XML { get; set; }


        public static InformacaoXML ObterXML(int idRetornoSat)
        {
            var retorno = RetornoSAT.Carregar(idRetornoSat);
            if (retorno.TipoSolicitacaoSAT == null)
                throw new Exception($"idRetornoSat {idRetornoSat} não existe");

            var tipo = retorno.TipoSolicitacaoSAT.IDTipoSolicitacaoSAT;
            var arquivo = Encoding.UTF8.GetString(Convert.FromBase64String(retorno.arquivoCFeSAT));
            XDocument xml = XDocument.Parse(arquivo, LoadOptions.None);
            string nomeArquivo = retorno.chaveConsulta;

            if (tipo == 1)//envio
                nomeArquivo = string.Format("{0}{1}{2}", "AD", nomeArquivo.Replace("CFe", string.Empty), ".xml");
            else if (tipo == 2)//cacelamento
                nomeArquivo = string.Format("{0}{1}{2}", "ADC", nomeArquivo.Replace("CFe", string.Empty), ".xml");

            return new InformacaoXML
            {
                NomeArquivo = nomeArquivo,
                XMLString = arquivo,
                XML = xml,
                Tipo = tipo.Value
            };
        }
    }
}
