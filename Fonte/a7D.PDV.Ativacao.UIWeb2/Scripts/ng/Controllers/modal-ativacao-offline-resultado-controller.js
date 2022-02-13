angular.module('AtivacaoApp')
  .controller('ModalAtivacaoOfflineResultadoController', ModalAtivacaoOfflineResultadoController)

ModalAtivacaoOfflineResultadoController.$inject = ['$uibModalInstance', 'dados']

function ModalAtivacaoOfflineResultadoController($uibModalInstance, dados) {
  var self = this
  self.dados = dados
  self.fechar = function() {
    $uibModalInstance.close()
  }
}