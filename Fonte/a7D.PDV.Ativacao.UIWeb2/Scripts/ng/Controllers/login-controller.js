angular
    .module('AtivacaoApp')
    .controller('LoginController', LoginController)

LoginController.$inject = ['$location', 'AuthenticationService', '$uibModal']

function LoginController($location, AuthenticationService, $uibModal) {
    var self = this
    self.errorMessage = null
    self.username = ''
    self.password = ''

    ;(function initController() {
        // reset login status
        AuthenticationService.ClearCredentials()
    })()

    self.login = function login() {
        self.dataLoading = true
        AuthenticationService.Login(self.username, self.password, function (response) {
            if (response.success) {
                AuthenticationService.SetCredentials(
                    self.username,
                    self.password,
                    response.contents.token,
                    response.contents.roles,
                    response.contents.perms
                );
                $location.path('/');
            } else {
                const err = response.contents || {};
                self.errorMessage =
                    (err.title || err.detail) ||
                    (err.errors && Object.values(err.errors).flat().join('\n')) ||
                    'Ocorreu um erro';
                self.dataLoading = false;
            }
        });
    }

    self.erro = function (err) {
        if (err.status === 401) {
            AuthenticationService.ClearCredentials()
        }
        self.errorMessage = err.data.Message || err.contents || 'Ocorreu um erro'
    }

    self.esqueci = function () {
        $uibModal.open({
            backdrop: 'static',
            templateUrl: 'static/template/modal-esqueci-senha-template.html',
            controller: 'ModalEsqueciSenhaController',
            controllerAs: 'vm',
            resolve: {
                email: function () {
                    return self.username
                }
            }
        }).result.then(function (data) {

        })
            .catch(self.erro)
    }
}
