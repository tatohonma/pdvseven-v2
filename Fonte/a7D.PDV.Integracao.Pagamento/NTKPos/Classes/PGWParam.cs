namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public class PGWParam
    {

        public int field;
        public string value;
        public PTIINF type => (PTIINF)field;

        public PGWParam(int Field, string FieldValue)
        {
            field = Field;
            value = FieldValue;
        }

        public PGWParam(int Field, int FieldValue)
        {
            field = Field;
            value = transformToString(FieldValue);
        }

        private string transformToString(int intNumber)
        {
            if (intNumber < 10)
            {
                return "0" + intNumber.ToString();
            }
            return intNumber.ToString();
        }

        public override string ToString()
        {
            return $"{field}: {value}";
        }
    }
}
