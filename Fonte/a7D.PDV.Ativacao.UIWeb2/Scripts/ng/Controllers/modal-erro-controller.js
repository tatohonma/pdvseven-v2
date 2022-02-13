angular.module('AtivacaoApp')
  .controller('ModalErroController', ModalErroController)

ModalErroController.$inject = ['$uibModalInstance', 'erro']

function ModalErroController($uibModalInstance, erro) {
  var self = this
  self.fechar = function() {
    $uibModalInstance.close()
  }
  self.erroTexto = 'Ocorreu um erro'
  if(typeof erro === 'string') {
    self.erroTexto = erro
  } else if (erro.data && erro.data.Message) {
    self.erroTexto = erro.data.Message
  } else if (erro.Message) {
    self.erroTexto = erro.Message
  }
}