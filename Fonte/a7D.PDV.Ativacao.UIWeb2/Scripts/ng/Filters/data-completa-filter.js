angular.module('AtivacaoApp').filter('dataCompleta', dataCompleta);

function dataCompleta() {
    return function (val) {
        return moment(val).format('DD/MM/YYYY HH:mm:ss');
    };
}


var tiposMensagens = [
    { id: 10, title: 'Update' },
    { id: 11, title: 'Update_Erro' },
    { id: 12, title: 'Update_SIM' },
    { id: 13, title: 'Update_ServerStart' },
    { id: 14, title: 'Update_ServerOK' },
    { id: 15, title: 'Update_ClienteOK' },

    { id: 30, title: 'Informacao' },
    { id: 31, title: 'Abrir_Link' },
    { id: 32, title: 'Pergunta_SIMNAO' },
    { id: 33, title: 'Pergunta_Texto' },
    { id: 34, title: 'Pergunta_Atualizar' },
    { id: 35, title: 'Cobranca_Link' },
    { id: 40, title: 'Resposta' },

    { id: 70, title: 'Config' }
];

angular.module('AtivacaoApp').filter('tipoMensagem', tipoMensagem);

function tipoMensagem() {
    return function (val) {
        for (var n = 0; n < tiposMensagens.length; n++)
            if (parseInt(val) === tiposMensagens[n].id)
                return tiposMensagens[n].title;

        return val;
    };
}