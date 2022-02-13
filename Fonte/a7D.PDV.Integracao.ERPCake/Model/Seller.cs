using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    public class Seller : ModelCake
    {
        public string name;
        public string person_type = "F";
        public string code;
        public bool? active;

        public override bool RequerAlteracaoPDV(DateTime dtSync, out int id)
            => !int.TryParse(code, out id);

        public override void SetCode(string code) => this.code = code;

        public override string ToString() => $"{id}: {name}";
    }
}
