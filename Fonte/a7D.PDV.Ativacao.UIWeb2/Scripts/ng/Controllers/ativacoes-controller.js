angular
    .module('AtivacaoApp')
    .controller('AtivacoesController', AtivacoesController);

AtivacoesController.$inject = [
    '$rootScope',
    'recursoAtivacao',
    'AuthenticationService',
    'NgTableParams',
    '$uibModal',
    'AccountContext',
    '$location',
];

function AtivacoesController(
    $rootScope,
    recursoAtivacao,
    AuthenticationService,
    NgTableParams,
    $uibModal,
    AccountContext,
    $location,
) {
    var vm = this;
    vm.mensagem = '';
    vm.ativacoes = [];
    

    vm.goEdit = function (id) {
        console.log($location.path('/ativacoes/edit/' + id));
        $location.path('/ativacoes/edit/' + id);
    };

    vm.ativofilter       = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }];
    vm.reativadoFilter   = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }];
    vm.duplicidadeFilter = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }];

    vm.hasRole    = function (role)     { return AccountContext.hasRole(role); };
    vm.hasAnyRole = function (required) { return AccountContext.hasAnyRole(required || []); };
    vm.hasPerm    = function (perm)     { return AccountContext.hasPermission(perm); };

    vm.isAdmin = AccountContext.hasRole('admin');
    $rootScope.$on('logado',    function(){ vm.isAdmin = AccountContext.hasRole('admin'); });
    $rootScope.$on('deslogado', function(){ vm.isAdmin = false; });

    vm.filters = {
        cliente: '',
        uf: '',
        cnpj: '',
        activationKey: '',
        isActive: '',
        isDuplicate: '',
        reactivatedBySupport: '',
        validityDays: '',
        createdFrom: '',
        createdTo: ''
    };

    vm.applyFilters = function () {
        var f = {};
        angular.forEach(vm.filters, function (v, k) {
            if (v !== null && v !== undefined && v !== '') f[k] = v;
        });
        vm.tableParams.page(1);
        vm.tableParams.filter(f);
    };

    vm.clearFilters = function () {
        angular.forEach(vm.filters, function (_, k) { vm.filters[k] = ''; });
        vm.applyFilters();
    };

    vm.tableParams = new NgTableParams(
        { page: 1, count: 25 },
        {
            getData: function (params) {
                return recursoAtivacao
                    .query(params.url(), function (data, headersGetter) {
                        var headers = headersGetter();
                        var total = parseInt(headers['count'], 10);
                        params.total(total);
                        return data;
                    }).$promise
                    .catch(vm.erro);
            }
        }
    );

    vm.erro = function (err) {
        if (err.status === 401) AuthenticationService.ClearCredentials();
        vm.mensagem = (err && err.data && (err.data.Message || err.data.message)) || 'Erro';
    };

    vm.acoes = function (ativacao) {
        $uibModal.open({
            backdrop: 'static',
            templateUrl: 'static/template/modal-acoes-ativacao-template.html',
            controller: 'ModalAcoesAtivacoesController',
            controllerAs: 'vm',
            resolve: { ativacao: function(){ return ativacao; } }
        }).result.then(angular.noop)
            .catch(vm.erro);
    };
}
