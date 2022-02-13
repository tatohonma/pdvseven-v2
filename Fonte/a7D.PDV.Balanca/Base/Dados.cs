using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Balanca
{
    public class Dados
    {
        public Dados()
        {

        }

        internal Dados(string conteudo, byte[] ascii, Status status, decimal peso)
        {
            Conteudo = conteudo;
            Ascii = ascii;
            Status = status;
            Peso = peso;
        }

        public string Conteudo { get; set; }
        public byte[] Ascii { get; set; }
        public Status Status { get; set; }
        public decimal Peso { get; set; }
    }
}
