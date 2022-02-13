using a7D.PDV.BigData.Shared.Model;
using a7D.PDV.BigData.Shared.ValueObject;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    [Table("Produto")]
    public class Produto : bdProduto
    {
        [Key, Column(Order = 1), Required, ForeignKey("Entidade")]
        public int IDEntidade { get; set; }

        public virtual Entidade Entidade { get; set; }

        internal void UpdateWith(bdProduto source)
        {
            Nome = source.Nome;
            EAN = source.EAN;
            EstoqueAtual = source.EstoqueAtual;
            Valor = source.Valor;
            Custo = source.Custo;
            ControlarEstoque = source.ControlarEstoque;
            Ativo = source.Ativo;
            EstoqueMinimo = source.EstoqueMinimo;
            EstoqueIdeal = source.EstoqueIdeal;
            dtAlteracao = source.dtAlteracao;
            // O Estoque atual sobe tudo separadamente apos sincronizar os produtos
        }
    }
}