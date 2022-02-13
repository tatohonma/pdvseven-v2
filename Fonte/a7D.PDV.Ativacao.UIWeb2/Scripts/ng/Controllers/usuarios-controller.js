angular.module('AtivacaoApp')
  .controller('UsuariosController', UsuariosController);

UsuariosController.$inject = ['recursoUsuario', 'AuthenticationService', 'NgTableParams', '$uibModal'];

function UsuariosController(recursoUsuario, AuthenticationService, NgTableParams, $uibModal) {
  var self = this;
  self.mensagem = null
  self.usuarios = []
  self.ativofilter = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }]
  self.idDeletar = null

  self.modal = null

  self.tableParams = new NgTableParams({}, {
    getData: function(params) {
      return recursoUsuario.query(params.url(), function(data, headersGetter) {
        var headers = headersGetter()
        var pages = parseInt(headers['count'], 10)
        params.total(pages)
        return data
      }).$promise
        .catch(self.erro)
    }
  })

  self.erro = function(err) {
    if (err.status === 401) {
      AuthenticationService.ClearCredentials()
    }
    self.mensagem = err.data.Message
  }

  self.resetarSenha = function(usuario) {
    $uibModal.open({
      backdrop: 'static',
      templateUrl: 'static/template/modal-custom-template.html',
      controller: 'ModalResetarSenhaController',
      controllerAs: 'vm',
      resolve: {
        usuario: function() {
          return usuario 
        }
      }
    }).result.then(function(data) {
      self.tableParams.reload()
    })
    .catch(self.erro)
  }

  self.reenviarEmail = function(usuario) {
    $uibModal.open({
      backdrop: 'static',
      templateUrl: 'static/template/modal-custom-template.html',
      controller: 'ModalReenviarEmailController',
      controllerAs: 'vm',
      resolve: {
        usuario: function() {
          return usuario 
        }
      }
    }).result.then(function(data) {
      self.tableParams.reload()
    })
    .catch(self.erro)
  }

  self.deletar = function(usuario) {
    $uibModal.open({
      backdrop: 'static',
      templateUrl: 'static/template/modal-custom-template.html',
      controller: 'ModalExcluirUsuarioController',
      controllerAs: 'vm',
      resolve: {
        usuario: function() {
          return usuario 
        }
      }
    }).result.then(function(data) {
      self.tableParams.reload()
    })
    .catch(self.erro)
  }
}