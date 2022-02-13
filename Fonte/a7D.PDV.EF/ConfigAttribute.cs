using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System;

namespace a7D.PDV.EF
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConfigAttribute : Attribute
    {
        public static Type[] TypeList;

        public ETipoPDV? TipoPDV { get; private set; }
        public string Valor { get; set; }
        public string ValoresAceitos { get; set; }
        public bool Obrigatorio { get; set; }
        public string Titulo { get; private set; }
        public EConfig Chave { get; set; }

        public ConfigAttribute(string titulo)
        {
            this.Titulo = titulo;
            this.TipoPDV = null;
        }

        public ConfigAttribute(string titulo, ETipoPDV tipo, EConfig chave = EConfig._none)
        {
            this.Titulo = titulo;
            this.TipoPDV = tipo;
            this.Chave = chave;
        }

        public tbConfiguracaoBD ToDB(string chave)
        {
            return new tbConfiguracaoBD()
            {
                Chave = chave,
                Valor = this.Valor,
                Titulo = this.Titulo,
                IDTipoPDV = TipoPDV == null ? null : (int?)TipoPDV.Value,
                ValoresAceitos = this.ValoresAceitos,
                Obrigatorio = this.Obrigatorio
            };
        }
    }
}
