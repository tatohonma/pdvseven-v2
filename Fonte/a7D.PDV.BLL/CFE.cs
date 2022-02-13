using a7D.PDV.BLL.Utils;
using System;
using System.IO;
using System.IO.Compression;

namespace a7D.PDV.BLL
{
    public class CFE
    {


        public static void EnviarZipParaWS(int[] idsRetornoSat, string nomeZip, DateTime dataDe, DateTime dataAte, string destinatario)
        {
            var FORMATODATA = "dd/MM/yyyy HH:mm:ss";
            var ms = new MemoryStream();

            using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {

                foreach (int idRetornoSat in idsRetornoSat)
                {
                    var xml = InformacaoXML.ObterXML(idRetornoSat);

                    var entry = zipArchive.CreateEntry(xml.NomeArquivo, CompressionLevel.Fastest);
                    using (var entryStream = entry.Open())
                    {
                        var sw = new StreamWriter(entryStream);// Encoding.GetEncoding("iso-8859-1")

                        sw.Write(xml.XMLString);
                        sw.Flush();
                    }
                }
            }

            ms.Position = 0;

            //var formatoDataZip = "ddMMyyyyHHmmss";
            var config = new ConfiguracoesSAT();
            var cnpj = config.InfCFe_emit_CNPJ;
            var nomeFantasia = config.infCFe_nome_fantasia;



            int len = (int)ms.Length;
            Byte[] mybytearray = new Byte[len];
            ms.Read(mybytearray, 0, len);
            ms.Close();

            // https://www.outlook-apps.com/maximum-email-size/
            if (mybytearray.Length > 20 * 1000 * 1000) // Limite de 20Mega
                throw new ExceptionPDV(CodigoErro.EAFE);



            var myservice = new Ativacoes.wsUtil();
            myservice.CFE(
                destinatario,
                nomeFantasia,
                cnpj,
                dataDe.ToString(FORMATODATA),
                dataAte.ToString(FORMATODATA),
                nomeZip,
                mybytearray
            );
        }
    }
}
