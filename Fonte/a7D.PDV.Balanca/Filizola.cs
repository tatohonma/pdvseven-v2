using System;
using System.IO.Ports;
using System.Text;

namespace a7D.PDV.Balanca
{
    public class Filizola : Balanca
    {
        internal Filizola()
        {
            conf = new Configuracao
            {
                BaudRate = 9600,
                DataBits = 8,
                Paridade = Parity.None,
                BitsDeParada = StopBits.Two,
                Autenticacao = Handshake.None,
                TimeoutEscrita = 2000,
                TimeoutLeitura = 2000,
                //Comando = "\u0005\r\n"
                Comando = ((char)5).ToString(),
                Tipo = ETipoBalanca.FILIZOLA
            };
        }

        protected override Dados InterpretarDados(string existing)
        {
            var data = Encoding.ASCII.GetBytes(existing);
            var status = Status.ERRO;
            string conteudo = "Erro na leitura";
            decimal peso = -1;
            if (data.Length == 7)
            {
                conteudo = Encoding.ASCII.GetString(data, 1, 5);
                switch (conteudo)
                {
                    case "IIIII":
                        status = Status.INSTAVEL;
                        break;
                    case "SSSSS":
                        status = Status.SOBREPESO;
                        break;
                    default:
                        status = Status.OK;
                        try
                        {
                            peso = (Convert.ToDecimal(conteudo) / 1000);
                        }
                        catch { }
                        break;
                }
            }
            return new Dados(conteudo, data, status, peso);
        }
    }
}
