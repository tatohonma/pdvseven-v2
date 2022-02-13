using System;

namespace a7D.PDV.EF
{
    /// <summary>
    /// Interface dos objetos que se integram com o ERP Generico (atualmente pensado no CakeERP)
    /// </summary>
    public interface IERP
    {
        /// <summary>
        /// Retorna ou define o código no ERP
        /// </summary>
        string CodigoERP { get; set; }
    }

    public interface IERPSync
    {
        /// <summary>
        /// Metodo para validar quando um registro precisa se enviado ao ERP 
        /// (a regra de validação pode depender de diversos campos, datas ou critérios fixos)
        /// </summary>
        /// <param name="dtSync">Data do ultimo sincronismo</param>
        /// <returns>Verdadeiro quando o item precisa ser sincronizado</returns>
        bool RequerAlteracaoERP(DateTime dtSync);

        int myID();
    }
}
