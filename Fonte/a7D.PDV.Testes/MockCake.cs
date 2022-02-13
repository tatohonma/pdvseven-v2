using a7D.PDV.Integracao.ERPCake;
using System;

namespace a7D.PDV.Testes
{
    public class MockCake : ICakeBase
    {
        public MockCake(DateTime dt)
        {
            api = new APIERPCake("1882ca0219610a51fba2");
            DataInicial = dt.AddDays(-15);
            UltimoSync = dt.AddSeconds(-30);
        }

        public APIERPCake api { get; private set; }

        public DateTime UltimoSync { get; private set; }

        public DateTime DataInicial { get; private set; }

        public int DefaultCustomer => 0;

        public void AddLog(string info) => Console.WriteLine(info);

        public void AddLog(Exception ex, bool saveLog = false) => throw ex;

        public void UpdateSync(DateTime dt) => UltimoSync = dt;
    }
}