using a7D.PDV.EF.Models;
using a7D.PDV.EF.Properties;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace a7D.PDV.EF.ValoresPadrao
{
    internal class ValoresRelatorios
    {
        internal static void Validar(pdv7Context context)
        {
            var todosRelatorios = new List<tbRelatorio>
            {
                // Tipo 1: Por fechamento
                new tbRelatorio() { IDTipoRelatorio=1, Ordem =  1, Nome = "Resumo dos Fechamentos", QuerySQL = "" },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem =  8, Nome = "Resumo por Tipo Pagamento", QuerySQL = Resources.Relatorio_Resumo_por_Tipo_Pagamento },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 20, Nome = "Resumo por Sexo", QuerySQL = Resources.Relatorio_Resumo_por_Sexo },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 30, Nome = "Pedidos com Desconto", QuerySQL = Resources.Relatorio_Pedidos_com_Desconto },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 40, Nome = "Lista de Pedidos", QuerySQL = Resources.Relatorio_Lista_de_Pedidos },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 50, Nome = "Produtos Cancelados", QuerySQL = Resources.Relatorio_Produtos_Cancelados },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 60, Nome = "Produtos Cancelados - Resumo por Usuário Lançamento", QuerySQL = Resources.Relatorio_Produtos_Cancelados_Usuario_Lancamento },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 70, Nome = "Produtos Cancelados - Resumo por Usuário Cancelamento", QuerySQL = Resources.Relatorio_Produtos_Cancelados_Usuario_Cancelamento },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 80, Nome = "Produtos Vendidos", QuerySQL = Resources.Relatorio_Produtos_Vendidos },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 90, Nome = "Produtos Vendidos - Resumo", QuerySQL = Resources.Relatorio_Produtos_Vendidos_Resumo },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 10, Nome = "Resumo por Tipo de Entrada", QuerySQL = Resources.Relatorio_Resumo_por_Tipo_de_Entrada },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 130, Nome = "Desconto por cliente", QuerySQL = Resources.Relatorio_Desconto_por_cliente },
                new tbRelatorio() { IDTipoRelatorio=1, Ordem = 140, Nome = "Motivo de Desconto", QuerySQL = Resources.Relatorio_Motivo_Desconto },
                // Tipo 2: Por data
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 10, Nome = "Lista de Clientes", QuerySQL = Resources.Relatorio_Lista_de_Clientes },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 20, Nome = "Produtos Vendidos", QuerySQL = Resources.Relatorio_Produtos_Vendidos_2 },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 30, Nome = "Produtos Vendidos - Resumo", QuerySQL = Resources.Relatorio_Produtos_Vendidos_Resumo_2 },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 40, Nome = "Produtos Vendidos por Cliente", QuerySQL = Resources.Relatorio_Produtos_Vendidos_por_Cliente },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 100, Nome = "Taxa de serviço por garçom", QuerySQL = Resources.Relatorio_Taxa_de_servico_por_garcom },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 110, Nome = "Taxa de Entrega Delivery", QuerySQL = Resources.Relatorio_Taxa_de_Entrega_Delivery },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 150, Nome = "Saidas Avulsas", QuerySQL = Resources.Relatorio_Saidas_Avulsas },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 160, Nome = "Sangria", QuerySQL = Resources.Relatorio_Sangria },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 170, Nome = "Taxa Entrega Delivery", QuerySQL = Resources.Relatorio_Taxa_Entrega_Delivery },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 180, Nome = "Taxa de Servico Com e Sem Desconto", QuerySQL = Resources.Relatorio_Taxa_Servico_Com_e_Sem_Desconto },
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 190, Nome = "Histórico de Créditos por Cliente", QuerySQL = Resources.Relatorio_Creditos},
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 200, Nome = "Formas de pagamento detalhado", QuerySQL = Resources.Relatorio_Tipos_Metodo_Bandeira},
                new tbRelatorio() { IDTipoRelatorio=2, Ordem = 210, Nome = "Impostos dos produtos", QuerySQL = Resources.Relatorio_Lista_Produtos_Impostos}
            };

            int i = 0;
            var atualRelatorios = context.tbRelatorios.ToList();
            while (i < todosRelatorios.Count)
            {
                // A atualização e verificação é por nome exato!!!
                var relatorio = atualRelatorios.FirstOrDefault(c => c.Nome == todosRelatorios[i].Nome && c.IDTipoRelatorio == todosRelatorios[i].IDTipoRelatorio);
                if (relatorio == null)
                    context.tbRelatorios.Add(todosRelatorios[i]);
                else if (relatorio.QuerySQL != todosRelatorios[i].QuerySQL)
                    relatorio.QuerySQL = todosRelatorios[i].QuerySQL;

                i++;
            }
        }
    }
}
