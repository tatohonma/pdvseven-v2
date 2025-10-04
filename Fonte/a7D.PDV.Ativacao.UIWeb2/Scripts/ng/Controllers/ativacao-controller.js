angular.module('AtivacaoApp')
    .controller('AtivacaoController', AtivacaoController);

AtivacaoController.$inject = [
    '$scope', '$rootScope', '$filter', '$location', '$routeParams', '$timeout',
    'recursoClientes', 'recursoAtivacao', 'recursoGerarChaveAtivacao',
    'cadastroAtivacao', 'AuthenticationService', 'recursoTipoPdv', '_',
    'AccountContext'
];

function AtivacaoController (
    $scope, $rootScope, $filter, $location, $routeParams, $timeout,
    recursoClientes, recursoAtivacao, recursoGerarChaveAtivacao,
    cadastroAtivacao, AuthenticationService, recursoTipoPdv, _,
    AccountContext
) {
    var vm = this;
    vm.carregado = false;
    vm.ativacao = {};
    vm.mensagem = null;
    vm.mensagemErr = null;
    vm.quantidade = 1;
    vm.tiposPdv = [];
    vm.filtro = '';

    // ---------- ADMIN por role (corrige o "volta pra lista") ----------
    if (!AccountContext.hasRole('admin')) {
        $location.path('/ativacoes');
        return;
    }

    // ---------- util: mapeamentos API <-> View ----------
    function fromApi(a) {
        a = a || {};
        return {
            id: a.id || 0,
            activationKey: a.activationKey || '',
            lastCheckedAt: a.lastCheckedAt || null,
            activatedAt: a.activatedAt || null,
            validityDays: a.validityDays || 0,
            isActive: !!a.isActive,
            reactivatedBySupport: !!a.reactivatedBySupport,
            supportReactivatedAt: a.supportReactivatedAt || null,
            provisionalValidityUntil: a.provisionalValidityUntil || null,
            isDuplicate: !!a.isDuplicate,
            notes: a.notes || '',
            clientId: a.clientId || (a.client && a.client.id) || null,
            client: a.client || null,
            pdVs: (a.pdVs || []).map(function(p){
                return {
                    id: p.id,
                    activationId: p.activationId,
                    installationPdvId: p.installationPdvId,
                    pdvTypeId: p.pdvTypeId,
                    name: p.name,
                    hardwareKey: p.hardwareKey,
                    version: p.version,
                    isActive: !!p.isActive,
                    updatedAt: p.updatedAt
                };
            })
        };
    }

    function toApi(v) {
        v = v || {};
        return {
            id: v.id || 0,
            activationKey: v.activationKey,
            lastCheckedAt: v.lastCheckedAt,
            activatedAt: v.activatedAt,
            validityDays: v.validityDays,
            isActive: !!v.isActive,
            reactivatedBySupport: !!v.reactivatedBySupport,
            supportReactivatedAt: v.supportReactivatedAt,
            provisionalValidityUntil: v.provisionalValidityUntil,
            isDuplicate: !!v.isDuplicate,
            notes: v.notes,

            clientId: (v.client && v.client.id) || v.clientId || null,

            pdVs: (v.pdVs || []).map(function (p) {
                return {
                    id: p.id,
                    installationPdvId: p.installationPdvId,
                    pdvTypeId: p.pdvTypeId || (p.pdvType && p.pdvType.id) || null,
                    name: p.name,
                    hardwareKey: p.hardwareKey,
                    version: p.version,
                    isActive: !!p.isActive,
                    updatedAt: p.updatedAt
                };
            })
        };
    }



    // ---------- Tipos de PDV (lista para nomePDV) ----------
    recursoTipoPdv.query(function (dados) {
        vm.tiposPdv = dados || [];
    });

    // ---------- Carregar edição ----------
    if ($routeParams.id) {
        recursoAtivacao.get({ id: $routeParams.id }).$promise
            .then(function (ativ) {
                console.log('[edit:data]', ativ);
                vm.ativacao = fromApi(ativ);
                vm.carregado = true;
            })
            .catch(function (err) {
                vm.carregado = true;
                if (err.status === 401) AuthenticationService.ClearCredentials();
                vm.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Registro não encontrado';
            });
    } else {
        vm.ativacao = {
            id: 0,
            activationKey: '',
            validityDays: 30,
            isActive: true,
            reactivatedBySupport: false,
            notes: '',
            client: null,
            pdVs: []
        };
        vm.carregado = true;
    }

    vm.salvar = function () {
        vm.enviando = true;
        var payload = toApi(vm.ativacao);

        cadastroAtivacao.salvar(payload)
            .then(function (ret) {
                vm.mensagem = 'Salvo com sucesso!';
                var data = ret.data || {};
                vm.ativacao = fromApi(data);
                if (ret.save === true && data.id) {
                    // manter na rota de edição do novo id (se for create)
                    $location.path('/ativacoes/edit/' + data.id);
                }
                vm.enviando = false;
            })
            .catch(function (err) {
                if (err.status === 401) AuthenticationService.ClearCredentials();
                vm.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Erro ao salvar';
                vm.enviando = false;
            });
    };

    vm.gerarChave = function () {
        var resellerId = (vm.ativacao.client && vm.ativacao.client.resellerId) || null;
        if (!resellerId) {
            vm.mensagemErr = 'Selecione um cliente com Revenda definida para gerar chave.';
            return;
        }

        recursoGerarChaveAtivacao.get({ resellerId: resellerId }).$promise
            .then(function (resp) {
                vm.ativacao.activationKey = resp.ActivationKey || resp.activationKey || vm.ativacao.activationKey;
            })
            .catch(function (err) {
                if (err.status === 401) AuthenticationService.ClearCredentials();
                vm.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Erro ao gerar chave';
            });
    };

    vm.adicionarLicenca = function (pdvTypeId) {
        vm.ativacao.pdVs = vm.ativacao.pdVs || [];
        var qtd = Number(vm.quantidade || 1);
        for (var i = 0; i < qtd; i++) {
            vm.ativacao.pdVs.unshift({
                id: 0,
                activationId: vm.ativacao.id || 0,
                installationPdvId: null,
                pdvTypeId: pdvTypeId,
                name: (vm.nomePDV(pdvTypeId) || 'PDV') + ' ' + (vm.ativacao.pdVs.length + 1),
                hardwareKey: '',
                version: '',
                isActive: true,
                updatedAt: null
            });
        }
        vm.quantidade = 1;
    };

    vm.alterarStatus = function (licenca) {
        licenca.isActive = !licenca.isActive;
    };

    vm.fecharErro = function () { vm.mensagemErr = null; };
    vm.fecharMensagem = function () { vm.mensagem = null; };

    vm.getClientes = function (val) {
        // ajuste: autocomplete esperando campos novos
        return recursoClientes.query({ 'filter[name]': val }).$promise.then(function (clientes) {
            return clientes; // deve vir {id, name, ...}
        });
    };

    vm.nomePDV = function (pdvTypeId) {
        // suporta objetos antigos (IDTipoPDV/Nome) e novos (id/name)
        for (var i = 0; i < vm.tiposPdv.length; i++) {
            var t = vm.tiposPdv[i];
            if ((t.IDTipoPDV || t.id) == pdvTypeId) return t.Nome || t.name;
        }
        return 'desconhecido(' + pdvTypeId + ')';
    };
}
