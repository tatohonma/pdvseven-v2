using System;
using System.IO.Ports;
using System.Text;

namespace a7D.PDV.Balanca
{
    public sealed class Toledo : Balanca
    {
        internal Toledo()
        {
            conf = new Configuracao
            {
                BaudRate = 9600,
                DataBits = 8,
                Paridade = Parity.None,
                BitsDeParada = StopBits.One,
                Autenticacao = Handshake.None,
                TimeoutEscrita = 450,
                TimeoutLeitura = 450,
                //Comando = "\u0005\r\n"
                Comando = ((char)5).ToString(),
                Tipo = ETipoBalanca.TOLEDO
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
