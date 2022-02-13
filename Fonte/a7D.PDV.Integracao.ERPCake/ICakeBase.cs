using System;

namespace a7D.PDV.Integracao.ERPCake
{
    public interface ICakeBase
    {
        DateTime DataInicial { get; }
        APIERPCake api { get; }
        DateTime UltimoSync { get; }
        int DefaultCustomer { get; }
        void AddLog(string info);
        void AddLog(Exception ex, bool saveLog = false);
        void UpdateSync(DateTime dt);
    }
}
