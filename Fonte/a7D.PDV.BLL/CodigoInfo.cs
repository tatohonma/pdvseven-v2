using System;
using System.ComponentModel;

namespace a7D.PDV.BLL
{
    [Serializable]
    public enum CodigoInfo
    {
        [Description("Aplicação iniciada")] I000,
        [Description("Transferência")] I001,
        [Description("Cancelamento Balcão")] I002,
        [Description("Resultado da Migracao")] I003,
        [Description("Pedido fechado não cancelado")] I004,
        [Description("iFood importado pedido")] I005,
        [Description("iFood atualizou cliente")] I006,
        [Description("iFood pedido atualizado")] I007,
        [Description("Cancelamento de item")] I008,
        [Description("Agrupamento")] I009,
        [Description("ERP Pedido")] I010,
        [Description("Fechamento de Comandas")] I012,
        [Description("Liquidação de Creditos")] I013,
    }
}
