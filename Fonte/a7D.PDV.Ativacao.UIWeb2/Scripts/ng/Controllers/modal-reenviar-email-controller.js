angular.module('AtivacaoApp')
  .controller('ModalReenviarEmailController', ModalReenviarEmailController)

ModalReenviarEmailController.$inject = ['$uibModalInstance', 'recursoUsuario', 'usuario']

function ModalReenviarEmailController($uibModalInstance, recursoUsuario, usuario) {
  var self = this;
  self.usuario = usuario
  self.processando = false
  self.modal = {
    titulo: 'Reenviar e-mail',
    mensagem: 'Reenviar o e-mail de cadastro para',
    fechar: function() {
      $uibModalInstance.close()
    },
    negativo: {
      acao: function() {
        $uibModalInstance.close()
      },
      classe: 'btn-default',
      texto: 'Não'
    }, 
    positivo: {
      acao: function() {
        self.processando = true
        self.processando = true
        recursoUsuario.reenviarEmail(null, self.usuario.Email).$promise
          .then(function(data) {
            $uibModalInstance.close()
          })
          .catch(function(err) {
            $uibModalInstance.dismiss(err)
          })
      },
      classe: 'btn-primary',
      texto: 'Sim'
    }
  }
}