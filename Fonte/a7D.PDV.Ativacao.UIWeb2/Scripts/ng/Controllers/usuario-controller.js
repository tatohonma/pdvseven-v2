angular.module('AtivacaoApp')
  .controller('UsuarioController', UsuarioController);

UsuarioController.$inject = ['$routeParams', '$location', '$timeout', 'recursoUsuario', 'cadastroUsuario', 'recursoEmail','makeCancelable', 'AuthenticationService'];

function UsuarioController($routeParams, $location, $timeout, recursoUsuario, cadastroUsuario, recursoEmail, makeCancelable, AuthenticationService) {
  var self = this
  self.carregado = false
  self.editando = false
  self.valido = true
  self.mensagemErr = null

  self.promise = null

  self.usuario = {
    Ativo: true,
    Adm: false,
    Email: '',
    Nome: '',
    CadastroPendente: false,
    Excluido: false
  }

  if($routeParams.id) {
    self.editando = true
    recursoUsuario.get({ id: $routeParams.id }).$promise
      .then(function(data) {
        self.usuario = data
        self.carregado = true
      })
      .catch(function(err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        self.mensagemErr = err.data.Message
        self.carregado = true
      })
  } else {
    self.carregado = true
  }

  self.salvar = function() {
    self.enviando = true
    cadastroUsuario.salvar(self.usuario)
      .then(function(ret) {
        self.mensagem = 'Salvo com sucesso!'
        if (ret.save === true) {
          self.carregado = false
          debugger;
          $location.path('/usuarios/edit/' + ret.data.IDUsuario)
        }
        self.ativacao = ret.data
        self.enviando = false
      })
      .catch(function(err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        self.mensagemErr = err.data.Message
        self.enviando = false
      })
  }

  self.checkEmail = function() {
    self.valido = true
    if(self.promise != null)
      self.promise.cancel()
    self.promise = makeCancelable(recursoEmail.existe({ email: self.usuario.Email, id: self.usuario.IDUsuario }).$promise)
    self.promise.promise.then(function(data) {
      self.valido = true
    })
    self.promise.promise.catch(function (err) {
      if(err.isCanceled) {
      }
      self.valido = false
    })
  }
}