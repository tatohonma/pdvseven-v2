using System;

namespace a7D.PDV.Integracao.iFood
{
    public class EventoID
    {
        public string id;
    }

    public class Evento : EventoID
    {
        public string code;
        public long correlationId;
        public DateTime createdAt;
    }

    /* https://developer.ifood.com.br/reference#eventspolling
    [{
        "code": "PLACED",
        "correlationId": "1234567890012",
        "createdAt": "2017-05-02T16:01:16.567Z",
        "id": "abc-456-afge-451-n15484"
      },
      {
        "code": "CANCELLED",
        "correlationId": "9876543210123",
        "createdAt": "2017-05-02T16:01:16.567Z",
        "id": "kfg-234-34fg-3523-jkf1515"
      }]
    */
}
