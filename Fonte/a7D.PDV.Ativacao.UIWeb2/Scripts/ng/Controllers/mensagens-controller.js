angular
    .module('AtivacaoApp')
    .controller('MensagensController', MensagensController);

MensagensController.$inject = ['$rootScope', 'recursoMensagens', 'NgTableParams', '$route', '$location'];
function MensagensController($rootScope, recursoMensagens, NgTableParams, $route, $location) {
    var self = this;
    self.adm = $rootScope.globals.currentUser.adm === true;

    if (!self.adm) {
        $location.path('/login');
        return;
    }

    self.mensagem = '';
    self.tiposFilter = tiposMensagens;
    self.tableParams = new NgTableParams({}, {
        getData: function (params) {
            return recursoMensagens.query(params.url(), function (data, headersGetter) {
                var headers = headersGetter();
                var pages = parseInt(headers['count'], 10);
                params.total(pages);
                return data;
            }).$promise.catch(self.erro);
        }
    });

    self.parametros = [
        { label: 'Versão 2.18.0.31', tipo: 'Update', parametro: 'http://www.pdvseven.com.br/download/update-2.18.0.31.zip' },
        { label: 'Versão 2.20.4', tipo: 'Update', parametro: 'http://www.pdvseven.com.br/download/update-2.20.4.zip' },
        { label: 'Versão 2.21.1', tipo: 'Update', parametro: 'http://www.pdvseven.com.br/download/update-2.21.1.zip' },
        { label: 'Pergunta Atualização 2.20.4', tipo: 'Pergunta_Atualizar', parametro: 'http://www.pdvseven.com.br/download/update-2.20.4.zip' },
        { label: 'Pergunta Atualização 2.21.1', tipo: 'Pergunta_Atualizar', parametro: 'http://www.pdvseven.com.br/download/update-2.21.1.zip' },
        { label: 'Enviar Informação', tipo: 'Informacao' },
        { label: 'Enviar Link', tipo: 'Abrir_Link' },
        { label: 'Enviar pergunda SIM/NÃO', tipo: 'Pergunta_SIMNAO' },
        { label: 'Enviar cobrança', tipo: 'Cobranca_Link' }
    ];

    self.ativacao = '';
    self.texto = '';
    self.parametro = self.parametros[0];
    self.parametroTexto = '';

    self.sendUpdate = function () {
        if (self.texto === "") {
            alert('Informe uma mensagen (texto) para o cliente');
            return;
        }

        console.log(self.parametro);

        var dados = {
            "Chave": self.ativacao, // 001-06079-14
            "Tipo": self.parametro.tipo,
            "Origem": "Ativador",
            "Destino": "Integrador",
            "Texto": self.texto,
            "Parametros": self.parametro.parametro || self.parametroTexto
        };

        console.log(dados);
        self.ativacao = '';

        recursoMensagens.message(dados, function (data) {
            console.log(data);
            $route.reload();
        }).$promise.catch(function (erro) {
            console.log(erro);
            var mensagem = erro.Message;
            var modelState = erro.data.ModelState;
            if (modelState) {
                try {
                    mensagem = (modelState['mensagem.Chave'] || modelState['Chave']).join("\r\n");
                }
                catch (e) {
                    mensagem += "\r\nVerifique outros erros console";
                }
            }
            alert(mensagem);
        });
    };
}
