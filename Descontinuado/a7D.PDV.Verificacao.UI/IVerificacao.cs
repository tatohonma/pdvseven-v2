using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Verificacao.UI
{
    public interface IVerificacao
    {
        bool Invalido { get; }
        string Mensagem { get; }
        string Nome { get; }
        CategoriaVerificacao Categoria { get; }
        Nivel Nivel { get; }
    }
}
