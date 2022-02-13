namespace a7D.PDV.Integracao.Loggi.Model
{
    public class ListResponse<T> : ErroLoggi
    {
        public Meta meta;
        public T[] objects;
    }
}
