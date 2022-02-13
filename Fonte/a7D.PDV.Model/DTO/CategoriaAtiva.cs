using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.DTO
{
    public class CategoriaAtiva
    {
        public int IDCategoriaProduto { get; set; }
        public string Nome { get; set; }
        public int Ativos { get; set; }
        public int Inativos { get; set; }

        public EstadoCategoria Estado
        {
            get
            {
                if (Ativos == 0)
                    return EstadoCategoria.Inativo;
                if (Inativos == 0)
                    return EstadoCategoria.Ativo;
                return EstadoCategoria.Intermediario;
            }
        }
    }

    public enum EstadoCategoria
    {
        Ativo,
        Inativo,
        Intermediario
    }
}
