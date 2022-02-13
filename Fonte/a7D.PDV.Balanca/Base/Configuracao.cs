using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Balanca
{
    internal class Configuracao
    {
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Paridade { get; set; }
        public StopBits BitsDeParada { get; set; }
        public Handshake Autenticacao { get; set; }
        public string Comando { get; set; }
        public int TimeoutLeitura { get; set; }
        public int TimeoutEscrita { get; set; }
        public ETipoBalanca Tipo { get; set; }
    }
}
