namespace a7D.PDV.Integracao.Loggi.Model
{
    public class ErroLoggi
    {
        //{"error_message": "Sorry, this request could not be processed. Please try again later."}
        public ErroList errors;
        public string error_message;
        public string last_request;
        public string last_result;

        public ErroLoggi()
        {
        }

        public ErroLoggi(string erro)
        {
            errors = new ErroList(erro);
        }

        public override string ToString()
            => errors.ToString() ?? error_message;
    }

    public class ErroList
    {
        // {"errors": {"__all__": ["Endere\u00e7o Rua Oscar Freired, 300 - , S\u00e3o Paulo, SP n\u00e3o encontrado."]}}
        public string[] __all__;

        // {"errors": {"package_type": ["Fa\u00e7a uma escolha v\u00e1lida. Grande n\u00e3o \u00e9 uma das escolhas dispon\u00edveis."], "__original": {"package_type": ["Fa\u00e7a uma escolha v\u00e1lida. Grande n\u00e3o \u00e9 uma das escolhas dispon\u00edveis."]}, "__all__": ["Verificar erros indicados"]}}
        public string[] package_type;

        public ErroList __original;

        private string erro;

        public ErroList()
        {
        }

        public ErroList(string erro)
        {
            this.erro = erro;
        }

        public override string ToString()
        {
            string errocompleto = erro;

            if (__all__ != null)
                errocompleto += string.Join("\r\n", __all__) + " ";

            if (package_type != null)
                errocompleto += string.Join("\r\n", package_type) + " ";

            if (__original != null)
                errocompleto += __original.ToString() + " "; ;

            return string.IsNullOrEmpty(errocompleto) ? null : errocompleto;
        }
    }
}