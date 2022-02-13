angular.module('AtivacaoApp')
  .controller('CadastroController', CadastroController);

CadastroController.$inject = ['$routeParams', '$location', 'recursoUsuario', 'AuthenticationService'];

function CadastroController($routeParams, $location, recursoUsuario, AuthenticationService) {
  var self = this
  var hash = $routeParams.hash

  self.erro = null
  self.carregando = true

  self.usuario = {
  }

  recursoUsuario.obterPorHash({hash: hash}, function(usuario) {
    self.usuario = usuario
    self.carregando = false
  }, function erro(err) {
    debugger;
    self.carregando = false
    self.erro = 'Não encontrado.'
    console.error(err)
  })

  self.completar = function() {
    self.erro = null
    self.carregando = true
    var senha = self.usuario.Senha
    recursoUsuario.alterarSenhaHash({ hash: hash }, self.usuario, function(usuario) {
      AuthenticationService.Login(usuario.Email, senha, function (response) {
        if (response.success) {
          AuthenticationService.ClearCredentials()
          $location.path('/')
        } else {
          vm.erro = response.contents || 'Ocorreu um erro'
          vm.carregando = false
        }
      })
    }, function erro(err) {
    debugger;
    self.carregando = false
    self.erro = 'Não encontrado.'
    console.error(err)
  })
  }
}