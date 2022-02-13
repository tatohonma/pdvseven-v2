namespace a7D.PDV.Integracao.Servico.Core
{
    public class apiError
    {
        public string error;
        public string error_description;
        public string exception;
        public string message;

        public virtual string GetMessage() => message ?? error_description ?? error;
    }

    // {"error":"unauthorized","error_description":"Client not fount for id: pdvsevn"}
}