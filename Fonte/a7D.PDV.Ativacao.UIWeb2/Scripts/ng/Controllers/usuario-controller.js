angular.module('AtivacaoApp')
    .controller('UsuarioController', UsuarioController);

UsuarioController.$inject = [
    '$routeParams', '$location', '$timeout',
    'recursoUsuario', 'cadastroUsuario', 'recursoEmail',
    'makeCancelable', 'AuthenticationService'
];

function UsuarioController(
    $routeParams, $location, $timeout,
    recursoUsuario, cadastroUsuario, recursoEmail,
    makeCancelable, AuthenticationService
) {
    var vm = this;
    vm.carregado = false;
    vm.editando = false;
    vm.valido = true;
    vm.mensagemErr = null;
    vm.mensagem = null;
    vm.enviando = false;
    vm.promise = null;

    // Mapeamento helper (se precisar adaptar algo)
    function fromApi(u) {
        u = u || {};
        return {
            id: u.id || 0,
            email: u.email || '',
            name: u.name || '',
            // aceita tanto "active" quanto "isActive" vindos do back
            active: typeof u.active === 'boolean' ? u.active : !!u.isActive,
            admin:  typeof u.admin  === 'boolean' ? u.admin  : !!u.isAdmin,
            pendingRegistration: !!(u.pendingRegistration || u.isPendingRegistration),
            deleted: !!(u.deleted || u.isDeleted)
        };
    }

    function toApiCreate(v) {
        return {
            name: v.name,
            email: v.email
        };
    }

    function toApiUpdate(v) {
        return {
            id: v.id || undefined,
            name: v.name,
            isActive: !!v.active,
            isAdmin:  !!v.admin
        };
    }
    

    vm.usuario = fromApi({ active: true, admin: false });

    if ($routeParams.id) {
        vm.editando = true;
        recursoUsuario.get({ id: $routeParams.id }).$promise
            .then(function (data) {
                vm.usuario = fromApi(data);
                vm.carregado = true;
            })
            .catch(function (err) {
                if (err.status === 401) AuthenticationService.ClearCredentials();
                vm.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Erro ao carregar';
                vm.carregado = true;
            });
    } else {
        vm.carregado = true;
    }

    vm.salvar = function () {
        vm.enviando = true;
        var isEdicao = !!vm.usuario.id;
        var payload = isEdicao ? toApiUpdate(vm.usuario) : toApiCreate(vm.usuario);

        cadastroUsuario.salvar(payload)
            .then(function (ret) {
                vm.mensagem = 'Salvo com sucesso!';
                var data = ret.data || {};
                // No PUT seu back retorna 204 NoContent, então "data" pode vir vazio
                if (ret.save === true && data.id) {
                    $location.path('/usuarios/edit/' + data.id);
                }
                // Recarrega o usuário se veio dado; senão mantém o atual
                if (data && (data.id || data.email)) vm.usuario = fromApi(data);
                vm.enviando = false;
            })
            .catch(function (err) {
                if (err.status === 401) AuthenticationService.ClearCredentials();
                vm.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Erro ao salvar';
                vm.enviando = false;
            });
    };

    vm.checkEmail = function () {
        vm.valido = true;

        if (vm.promise) vm.promise.cancel();

        vm.promise = makeCancelable(
            recursoEmail.existe({ email: vm.usuario.email, id: vm.usuario.id }).$promise
        );

        vm.promise.promise
            .then(function (livre) { vm.valido = !!livre; })
            .catch(function (err) {
                if (err && err.isCancelled) return;
                vm.valido = false;
            });
    };
}
