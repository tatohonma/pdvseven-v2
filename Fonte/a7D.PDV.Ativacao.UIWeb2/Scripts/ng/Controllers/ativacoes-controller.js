angular
  .module('AtivacaoApp')
  .controller('AtivacoesController', AtivacoesController)

AtivacoesController.$inject = ['$rootScope', 'recursoAtivacao', 'AuthenticationService', 'NgTableParams', '$uibModal']

function AtivacoesController($rootScope, recursoAtivacao, AuthenticationService, NgTableParams, $uibModal) {
  var self = this
  self.mensagem = ''
  self.ativacoes = []

  self.ativofilter = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }]
  self.reativadoFilter = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }]
  self.duplicidadeFilter = [{ id: 1, title: 'Sim' }, { id: 0, title: 'Não' }]

  self.adm = $rootScope.globals.currentUser.adm === true

  self.tableParams = new NgTableParams({}, {
    getData: function (params) {
      return recursoAtivacao.query(params.url(), function (data, headersGetter) {
        var headers = headersGetter()
        var pages = parseInt(headers['count'], 10)
        params.total(pages)
        return data
      }).$promise
        .catch(self.erro)
    }
  })

  self.erro = function(err) {
    if (err.status === 401)
      AuthenticationService.ClearCredentials()
    self.mensagem = err.data.Message
  }

  self.acoes = function(ativacao) {
    $uibModal.open({
      backdrop: 'static',
      templateUrl: 'static/template/modal-acoes-ativacao-template.html',
      controller: 'ModalAcoesAtivacoesController',
      controllerAs: 'vm',
      resolve: {
        ativacao: function() { return ativacao }
      }
    }).result.then(function(data) {
      
    })
    .catch(self.erro)
  }
}
