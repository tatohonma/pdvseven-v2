using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.iFood
{
    public class Token : apiError
    {
        public string access_token;
        public string token_type;
        public int expires_in;
        public string scope;
    }

    /*  https://developer.ifood.com.br/v1.0/docs
    {
     "access_token": "ab04421d-10d4-462c-b370-6456lrk897564aa48",
     "token_type": "bearer",
     "expires_in": 3599,
     "scope": "trust read write"
    } 
    */
}
