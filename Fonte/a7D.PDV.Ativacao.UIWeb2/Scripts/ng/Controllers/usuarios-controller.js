angular.module('AtivacaoApp')
    .controller('UsuariosController', UsuariosController);

UsuariosController.$inject = ['recursoUsuario', 'AuthenticationService', 'NgTableParams', '$uibModal'];

function UsuariosController(recursoUsuario, AuthenticationService, NgTableParams, $uibModal) {
    var self = this;

    self.mensagem = null;
    self.boolFilter = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }];

    self.filters = {
        email: '',
        name: '',
        active: '',
        admin: '',
        pendingRegistration: '',
        deleted: ''
    };

    self._buildFilter = function () {
        var f = {};
        if (self.filters.email)                f.Email = self.filters.email;
        if (self.filters.name)                 f.Name = self.filters.name;
        if (self.filters.active !== '' && self.filters.active != null)
            f.Active = self.filters.active;
        if (self.filters.admin !== '' && self.filters.admin != null)
            f.Admin = self.filters.admin;
        if (self.filters.pendingRegistration !== '' && self.filters.pendingRegistration != null)
            f.PendingRegistration = self.filters.pendingRegistration;
        if (self.filters.deleted !== '' && self.filters.deleted != null)
            f.Deleted = self.filters.deleted;
        return f;
    };

    self.tableParams = new NgTableParams(
        { page: 1, count: 10 },
        {
            getData: function (params) {
                var query = angular.extend({}, params.url(), self._buildFilter());

                return recursoUsuario.query(query, function (data, headersGetter) {
                    var headers = headersGetter();
                    var total = parseInt(headers['count'] || headers['Count'] || 0, 10);
                    params.total(isNaN(total) ? 0 : total);
                    return data;
                }).$promise.catch(self.erro);
            }
        }
    );

    self.applyFilters = function () {
        self.tableParams.page(1);
        self.tableParams.reload();
    };

    self.clearFilters = function () {
        self.filters = {
            email: '',
            name: '',
            active: '',
            admin: '',
            pendingRegistration: '',
            deleted: ''
        };
        self.applyFilters();
    };

    self.erro = function (err) {
        if (err && err.status === 401) {
            AuthenticationService.ClearCredentials();
            return;
        }
        self.mensagem = (err && err.data && (err.data.Message || err.data.message)) || 'Erro inesperado';
    };

    self.resetarSenha = function (usuario) {
        $uibModal.open({
            backdrop: 'static',
            templateUrl: 'static/template/modal-custom-template.html',
            controller: 'ModalResetarSenhaController',
            controllerAs: 'vm',
            resolve: { usuario: function () { return usuario; } }
        }).result.then(function () { self.tableParams.reload(); })
            .catch(self.erro);
    };

    self.reenviarEmail = function (usuario) {
        $uibModal.open({
            backdrop: 'static',
            templateUrl: 'static/template/modal-custom-template.html',
            controller: 'ModalReenviarEmailController',
            controllerAs: 'vm',
            resolve: { usuario: function () { return usuario; } }
        }).result.then(function () { self.tableParams.reload(); })
            .catch(self.erro);
    };

    self.deletar = function (usuario) {
        $uibModal.open({
            backdrop: 'static',
            templateUrl: 'static/template/modal-custom-template.html',
            controller: 'ModalExcluirUsuarioController',
            controllerAs: 'vm',
            resolve: { usuario: function () { return usuario; } }
        }).result.then(function () { self.tableParams.reload(); })
            .catch(self.erro);
    };
}
