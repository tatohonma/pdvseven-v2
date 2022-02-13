namespace a7D.PDV.Shared.Atualizacao
{
    delegate void UpdateText(string info);

    public interface IShowInfo
    {
        void ShowInfo(string info);
    }
}
