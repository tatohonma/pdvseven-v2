using System;
using System.IO;

namespace a7D.PDV.Integracao.Pagamento.NTKTEF
{
    internal class NTKWriter
    {
        private const string pathReq = @"C:\PAYGO\Req";
        private const string pathResp = @"C:\PAYGO\Resp";
        private const string fileTMP = @"\intpos.tmp";
        private const string fileReq = @"\intpos.001";
        private const string fileSts = @"\intpos.Sts";

        internal NTKWriter()
        {
            var di = new DirectoryInfo(pathReq);
            if (!di.Exists)
                throw new Exception("O PayGo não está instalado corretamente");

            var fi1 = new FileInfo(pathReq + fileTMP);
            if (fi1.Exists)
                fi1.Delete();

            var fi2 = new FileInfo(pathReq + fileReq);
            if (fi2.Exists)
                fi2.Delete();

            var fi3 = new FileInfo(pathResp + fileReq);
            if (fi3.Exists)
                fi3.Delete();

            var fi4 = new FileInfo(pathResp + fileSts);
            if (fi4.Exists)
                fi4.Delete();
        }

        internal void Write(NTKCampos campo, string valor)
        {
            Write(campo, 0, valor);
        }

        internal void Write(NTKCampos campo, int indice, string valor)
        {
            valor = valor.RemoveAcentos();
            if (indice > 0)
                valor = "\"" + valor + "\"";

            // 6.1. Formato genérico
            string linha = $"{((int)campo).ToString("000")}-{indice.ToString("000")} = {valor}\r\n";

            // 5.11.1. Gravação de arquivo
            File.AppendAllText(pathReq + fileTMP, linha);
        }

        internal string Read()
        {
            return File.ReadAllText(pathReq + fileTMP);
        }

        internal void Commit()
        {
            File.AppendAllText(pathReq + fileTMP, "999-999 = 0"); // Códgo de finalização
            File.Move(pathReq + fileTMP, pathReq + fileReq); // Libera o arquivo para uso
        }
    }
}