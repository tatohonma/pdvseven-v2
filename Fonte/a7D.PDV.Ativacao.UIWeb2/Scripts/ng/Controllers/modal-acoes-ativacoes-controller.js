angular.module('AtivacaoApp')
  .controller('ModalAcoesAtivacoesController', ModalAcoesAtivacoesController)

ModalAcoesAtivacoesController.$inject = ['$uibModalInstance', 'recursoAtivacao', 'ativacao']

function ModalAcoesAtivacoesController($uibModalInstance, recursoAtivacao, ativacao) {
  var self = this

  self.ativacao = ativacao
  console.log(self.ativacao)

  self.fechar = function() {
    $uibModalInstance.close()
  }
}