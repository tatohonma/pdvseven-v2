using a7D.PDV.Integracao.Loggi;
using a7D.PDV.Integracao.Loggi.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestLoggi
    {
        APILoggi api;

        [TestInitialize]
        public void Start()
        {
            // https://staging.loggi.com/   (ambiente de testes)
            // Login: suporte@pdvseven.com.br
            // Senha: pdvseven15
            // https://staging.loggi.com/contas/haxor/   (acessar apos o login para obter o token)
            // Prefixo 'staging:' no login, chavei a API para usar o ambiente de teste
            api = new APILoggi("staging:suporte@pdvseven.com.br", "d62ee297801c1f9bc266ee7edd4cc991a559b084");
        }

        [TestMethod, TestCategory("Loggi")]
        public void Loggi_Estimativa_Consulta()
        {
            //var origem = new WayPointQuery("Rua 21 de Abril, 1001");
            //var origem = new WayPointQuery("Rua Cajuru, 74 ap 61 A");
            var origem = new WayPointQuery("Rua Ernesto de Oliveira, 361");
            //var origem = new WayPointQuery("01426000", 300);
            var destino = new WayPointQuery("Rua", "Costa Aguiar", 2403);
            var estimativa = new EstimativaRequest(origem, destino);
            var result = api.Estimativa(estimativa);
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            Console.WriteLine(result);
            Assert.IsNull(result.errors);
            // {"optimized": null, "optimized_slo": null, "normal_slo": {}, "normal": {"estimated_cost": "33.73", "distance": 10345, "original_eta": 2470, "path_suggested_gencoded": "x`unChtm{Gi@yBfBaCtBuC`ByBz@fBnC~Fv@`BBPAPCLOT_An@EJ@TfBvFdAhDz@lCo@d@_BpAyC`CuFnEgFdEdDbGdCvEz@xAuAv@cAr@e@b@AV?V?RG^O~@AL?HHRJTf@hA~AxCNb@F\\v@jFx@tFP|A?^AJBN^hCjAfJT|BLd@JdA^rD@R@fBuCp@yAPiB@kAFWDc@P]Va@`@aAjA}BxCiB`C}@bAc@p@GV]|BcAjGg@`DExDExBAFJTDDD@rFWxBKREh@ZnAn@bAt@z@r@d@ZlBbArCnAnAt@dLtGdAn@hB~@dCxAX\\f@d@Zj@Xx@\\pAJp@F~@Dl@CXJb@L^v@vBdAdCp@bBr@rBdAfDf@jBn@jCj@jCVv@\\h@NRh@`@b@VXD`Br@hBx@rGxC|@f@j@j@bB`CvCnEbDpGx@pArAdBNRNRtAbB~@t@pA`A@RPf@~@bAhBtBNJRB`@EZIXOF?F@FBD@D@@?@VCr@DZdApA{@~@}FzGsDhEeIrJEPiBvByA`BqCzCYx@Gp@@`@F^~@dDTz@TfAFj@@f@Er@[pAiBlFoA`EeApDg@nBmAzDk@zAw@jCuB`HJ`@`@n@zChGbBbD"}}
        }

        [TestMethod, TestCategory("Loggi")]
        public void Loggi_GraphiQL_Query()
        {
            // https://staging.loggi.com/graphiql
            //var result = api.GraphiQL("query { allCities { edges { node { pk name slug } } } }");

            var result = api.GraphiQL("query{allShops{edges{node{name pk chargeOptions {label pk}}}}}");
            // {"data":{"allShops":{"edges":[{"node":{"name":"Pizza Hut Morumbi","pk":136,"chargeOptions":[{"label":"Dinheiro com troco","pk":8},{"label":"Dinheiro sem troco","pk":4},{"label":"M\u00e1quina da loja","pk":32},{"label":"Cheque","pk":16},{"label":"N\u00e3o h\u00e1 cobran\u00e7a","pk":64}]}}]}}}

            Console.WriteLine(result);
        }

        [TestMethod, TestCategory("Loggi")]
        public void Loggi_Pagamento_Incluir()
        {
            // Deve ter ao menos uma forma de pagamento
            var pagamento = new MetodoPagamento()
            {
                expiration_year = 2017,
                cvv = "092",
                number = "4111111111111111",
                expiration_month = "02",
                type = "c",
                billing_name = "ROBERT THE BRUCE"
            };
            api.MetodosPagamentoCriar(pagamento);
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
        }

        [TestMethod, TestCategory("Loggi")]
        public void Loggi_Orcamento_Pedido()
        {
            // Deve ter ao menos uma forma de pagamento
            /*
curl --request POST \                                1232ms  Wed 18 Jul 2018 08:39:19 AM -03
--url https://staging.loggi.com/graphql \
--header 'authorization: apiKey pizzahutmorumbi@testeloggi.com:5e742cb2b5cfaaf372fae73c5b4c
a6c5997d6383' \
--header 'content-type: application/json' \
--data '{"query":"query{allShops{edges{node{name pk chargeOptions {label pk}}}}}"}'
            */
            var pagamentos = api.MetodosPagamento();
            if (pagamentos == null || pagamentos.Length == 0)
                Assert.Inconclusive("Sem meios de pagamento cadastrados");

            foreach (var pagamento in pagamentos)
                Console.WriteLine(pagamento);

            //var origem = new WayPointQuery("04116170", 361, "Eureka - PDVSeven");
            var origem = new WayPointQuery("04116-170, 361");
            //var origem = new WayPointQuery("Rua Ernesto de Oliveira, 361");
            //var origem = new WayPointQuery("Rua Ernesto de Oliveira, 361"); // ?? Rua Pastor Antônio Ernesto de Oliveira, 361 Parque Primavera -São Paulo, SP
            origem.query.instructions = "origem do pedido";
            var destino = new WayPointQuery("Rua", "Cajuru", 74, "ap 61 A");
            destino.query.instructions = "pedido 123";
            var orcamento = new OrcamentoRequest(origem, destino);
            var result = api.Orcamento(orcamento);
            //var result = api.Pedido(orcamento);
            Console.WriteLine(result);
            // {"path_suggested_gencoded": "fgunCrnl{GqEpDIB?FqGxLsE|CnFfXtI_BnBeA\\w@xBgBrJ_GfIiFfXyRpB{@lB[nK\\lKj@xCHrFRlDc@bC}@rHeF~DmChDaCtD_DV@nCbDxG`Iv@t@fCsEnAyBtCgF~BbBV@xa@kOpNoF`SsH`GyB]tJ@pCOzGSzEChDTxArBtG`@dBz@xCf@rA^rFf@zO`@zL~DO|AIzCGN@BI`Ps@h[wABDHBFInGY", "city": 1, "total_time": null, "driver": null, "waypoints": [{"billable_minutes": 0, "completed_contact_name": null, "address_data": {"geometry": {"location": {"lat": -23.5416484, "lng": -46.59676899999999}, "viewport": {"northeast": {"lat": -23.5402994197085, "lng": -46.59542001970849}, "southwest": {"lat": -23.5429973802915, "lng": -46.59811798029149}}, "location_type": "ROOFTOP"}, "place_id": "ChIJM2QwRixZzpQRw9TQShHy69k", "provider": "google", "address_components": [{"long_name": "746", "types": ["street_number"], "short_name": "746"}, {"long_name": "Rua Cajuru", "types": ["route"], "short_name": "R. Cajuru"}, {"long_name": "Belenzinho", "types": ["political", "sublocality", "sublocality_level_1"], "short_name": "Belenzinho"}, {"long_name": "S\u00e3o Paulo", "types": ["administrative_area_level_2", "political"], "short_name": "S\u00e3o Paulo"}, {"long_name": "S\u00e3o Paulo", "types": ["administrative_area_level_1", "political"], "short_name": "SP"}, {"long_name": "Brazil", "types": ["country", "political"], "short_name": "BR"}, {"long_name": "03057-000", "types": ["postal_code"], "short_name": "03057-000"}], "formatted_address": "R. Cajuru, 746 - Belenzinho, S\u00e3o Paulo - SP, 03057-000, Brazil", "types": ["street_address"]}, "address_number": "746", "pos": "POINT (-46.5967689999999877 -23.5416483999999997)", "is_editable": false, "completed_at": null, "completed_contact_id": null, "approximate_location": null, "completed_pos": null, "id": 393189, "google_maps_id": null, "completed_status": 1, "index_display": "A", "index": 0, "vicinity": "Belenzinho", "state": "SP", "role": "ra", "zip": "03057000", "referees": [], "city": "S\u00e3o Paulo", "leg_distance": 9569, "arrived_pos": null, "address_st": "Rua Cajuru", "arrived_at": null, "is_removable": false, "address_formatted": "Rua Cajuru, 746 - Belenzinho, S\u00e3o Paulo - SP, 03057-000, Brasil", "instructions": "Retirar pedido #3", "completed_pos_distance": null, "address_complement": "", "canceled": false, "address_formated": "Rua Cajuru, 746 - Belenzinho, S\u00e3o Paulo - SP, 03057-000, Brasil", "notes": null, "favorite": "", "arrived_pos_distance": null, "e_receipt": {"status": "N\u00e3o dispon\u00edvel", "status_code": 1, "contact_name": null, "contact_id": null, "signature": null}, "leg_time": 1577}, {"billable_minutes": 0, "completed_contact_name": null, "address_data": {"geometry": {"location": {"lat": -23.5944894, "lng": -46.6057533}, "viewport": {"northeast": {"lat": -23.5931404197085, "lng": -46.60440431970849}, "southwest": {"lat": -23.5958383802915, "lng": -46.60710228029149}}, "location_type": "ROOFTOP"}, "place_id": "ChIJofVr3OxbzpQRW5EINRL45-o", "provider": "google", "address_components": [{"long_name": "2304", "types": ["street_number"], "short_name": "2304"}, {"long_name": "Rua Costa Aguiar", "types": ["route"], "short_name": "Rua Costa Aguiar"}, {"long_name": "Ipiranga", "types": ["political", "sublocality", "sublocality_level_1"], "short_name": "Ipiranga"}, {"long_name": "S\u00e3o Paulo", "types": ["administrative_area_level_2", "political"], "short_name": "S\u00e3o Paulo"}, {"long_name": "S\u00e3o Paulo", "types": ["administrative_area_level_1", "political"], "short_name": "SP"}, {"long_name": "Brazil", "types": ["country", "political"], "short_name": "BR"}, {"long_name": "04203-000", "types": ["postal_code"], "short_name": "04203-000"}], "formatted_address": "Rua Costa Aguiar, 2304 - Ipiranga, S\u00e3o Paulo - SP, 04203-000, Brazil", "types": ["street_address"]}, "address_number": "2304", "pos": "POINT (-46.6057533000000035 -23.5944894000000005)", "is_editable": false, "completed_at": null, "completed_contact_id": null, "approximate_location": null, "completed_pos": null, "id": 393190, "google_maps_id": null, "completed_status": 1, "index_display": "B", "index": 1, "vicinity": "Ipiranga", "state": "SP", "role": "ra", "zip": "04203000", "referees": [], "city": "S\u00e3o Paulo", "leg_distance": 0, "arrived_pos": null, "address_st": "Rua Costa Aguiar", "arrived_at": null, "is_removable": false, "address_formatted": "Rua Costa Aguiar, 2304 - Ipiranga, S\u00e3o Paulo - SP, 04203-000, Brasil", "instructions": "Entregar para Fabio Ferreira de Souza", "completed_pos_distance": null, "address_complement": "", "canceled": false, "address_formated": "Rua Costa Aguiar, 2304 - Ipiranga, S\u00e3o Paulo - SP, 04203-000, Brasil", "notes": null, "favorite": "", "arrived_pos_distance": null, "e_receipt": {"status": "N\u00e3o dispon\u00edvel", "status_code": 1, "contact_name": null, "contact_id": null, "signature": null}, "leg_time": 0}], "payment": {}, "customer": null, "slo": 1, "transport_type": "2", "pricing": {"total_cm": "31.87", "sum_wait_cm": "0.00", "total_cm_gross": "31.87", "applied_bonuses": [], "discount": "0.00", "bonuses": "0.00", "inquiry": "24c28aba-89f6-11e8-85e9-0242ac110004", "sum_ride_cm": "31.87", "id": 48643}, "city_name": "S\u00e3o Paulo", "original_eta": 2919, "id": "24c28aba-89f6-11e8-85e9-0242ac110004"}
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);

            Assert.IsNotNull(result.id);

            var pag = pagamentos.FirstOrDefault(); //.FirstOrDefault(p => p.type == "c");
            var result2 = api.PedidoConfirmar(result.id, pag.id.Value);
            Console.WriteLine(result2);

            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            // {"accepted": null, "allowRemoveWaypoints": true, "bundled": false, "can_bundle": true, "city": 1, "city_name": "São Paulo", "created": 1531934039, "crm_integrated_at": null, "customer": {"company": {"id": 60877, "landline": "11900000000", "landline2": null, "name": "Pizza Hut Morumbi"}, "email": "pizzahutmorumbi@testeloggi.com", "id": 296022, "is_staff": false, "is_superuser": false, "name": "Pizza Hut Morumbi", "telephone_number": "11900000000", "telephone_number2": null}, "distance": 254, "driver": null, "dropped": null, "finished": null, "finished_notes": "", "from_inquiry": "f11b8e92-8aad-11e8-9c9c-0242ac110005", "id": "47778", "itinerary": 29451, "must_start_before": null, "notes": "", "original_eta": 1554, "package_type": "medium_box", "path_suggested_gencoded": "rwtnCtqk{GfEjJ", "payment": {"captured": null, "captured_at": null, "method": "Padrão", "type": "b"}, "pricing": {"bonuses": "0.00", "discount": "0.00", "id": 28929, "order": 47778, "sum_ride_cm": "23.90", "sum_wait_cm": "0.00", "total_cm": "23.90", "total_cm_gross": "23.90"}, "product": 0, "product_display": "Corp", "product_version": 1, "real_completion_time": null, "receipt_uri": "/minhaloggi/recibos/47778/", "redo": {}, "resource_uri": "/api/v1/pedidos/47778/", "shared": "/c/idsRKDq2/", "slo": 1, "started": null, "status": "allocating", "total_time": null, "total_time_from_accepting": null, "transport_type": "2", "updated": 1531934039, "waypoints": [{"e_receipt": {"contact_id": null, "contact_name": null, "signature": null, "status": "Não disponível", "status_code": 1}, "vicinity": "Belenzinho"}, {"e_receipt": {"contact_id": null, "contact_name": null, "signature": null, "status": "Não disponível", "status_code": 1}, "vicinity": "Belenzinho"}]}

            Assert.IsNull(result.errors);
        }

        [TestMethod, TestCategory("Loggi")]
        public void Loggi_Pedidos()
        {
            //var result = api.PedidoListar();
            var result = api.PedidoStatus(47782);
            Console.WriteLine(result);
            // {"accepted": null, "company": {"id": 60877, "name": "Pizza Hut Morumbi"}, "completion_eta": 1531936128, "created": 1531934039, "customer_name": "Pizza Hut Morumbi", "driver": {}, "finished": null, "id": 47778, "is_pro": false, "origin": {"location": [-46.5946668, -23.5404284]}, "pricing": {"total_cm": "23.90", "total_dr": "18.90"}, "product_display": "Corp", "progress": 0, "slo_mode": 1, "started": null, "status": "allocating", "task": {"action": false, "steps_before": 0, "waypoint": 0}, "transport_type": "2", "waypoints": [{"address_formated": "Rua Cajuru, 1001 - Belenzinho, São Paulo - SP, 03057-000, Brasil - yyy", "address_formatted": "Rua Cajuru, 1001 - Belenzinho, São Paulo - SP, 03057-000, Brasil - yyy", "arrived_at": null, "arrived_at_eta": 1531935391, "completed_at": null, "completed_at_eta": 1531936090, "current": true, "e_receipt": {"contact_id": null, "contact_name": null, "signature": null, "status": "Não disponível", "status_code": 1, "unavailable_signature": "https://s3-sa-east-1.amazonaws.com/loggi-staging-static/images/stable/e-receipt/bg-protocol-fail-temp.gif"}, "id": 393806, "location": [-46.5946668, -23.5404284], "minutes_waiting": null, "minutes_waiting_overtime": null, "waypoints_before": 0}, {"address_formated": "Rua Cajuru, 746 - Belenzinho, São Paulo - SP, 03057-000, Brasil - xxx", "address_formatted": "Rua Cajuru, 746 - Belenzinho, São Paulo - SP, 03057-000, Brasil - xxx", "arrived_at": null, "arrived_at_eta": 1531936128, "completed_at": null, "completed_at_eta": 1531936827, "current": false, "e_receipt": {"contact_id": null, "contact_name": null, "signature": null, "status": "Não disponível", "status_code": 1, "unavailable_signature": "https://s3-sa-east-1.amazonaws.com/loggi-staging-static/images/stable/e-receipt/bg-protocol-fail-temp.gif"}, "id": 393807, "location": [-46.59676899999999, -23.5416484], "minutes_waiting": null, "minutes_waiting_overtime": null, "waypoints_before": 1}]}
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
        }

        [TestMethod, TestCategory("Loggi")]
        public void Loggi_AutoComplete()
        {
            //var result = api.PedidoListar();
            var result = api.AutoComplete("Rua Cajuru");
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
        }
    }
}