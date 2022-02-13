using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EContaRecebivel : int
    {
        Dinheiro = 10,
        Cielo = 20,
        Elavon = 30,
        Getnet = 40,
        PagSeguro = 50,
        Rede = 60,
        Stone = 70,
        Ticket = 80,
        Vero = 90,
        Todo = 100,
        Bin = 110,
        [Description("Conta Assinada")] ContaAssinada = 120,
        iFood = 130,
        [Description("Conta de Cliente")] ContaCliente= 140,
    }
}
