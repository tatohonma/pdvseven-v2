using System;

namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoKeyValue
    {
        public string Key { get; private set; }
        public object Value { get; private set; }

        private readonly string OriginalValue;

        private readonly DateTime DataAtualizada;

        public IaagoKeyValue(string key, object value, string orginal, DateTime? dt = null)
        {
            Key = key;
            Value = value;
            OriginalValue = orginal;
            DataAtualizada = dt ?? DateTime.Now;
        }

        public override string ToString() => $"{Key} = {Value} ({DataAtualizada})";
    }
}
