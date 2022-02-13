using a7D.PDV.Model;

namespace a7D.PDV.BLL.ValueObject
{
    public class PDVNomeID
    {
        public PDVNomeID(PDVInformation pdv)
        {
            id = pdv.IDPDV.Value;
            nome = pdv.Nome;
        }

        public int id;
        public string nome;
        public bool ativo;
        public override string ToString() => $"{id}: {nome}";
    }
}
