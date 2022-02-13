angular.module('AtivacaoApp')
  .controller('ModalResetarSenhaController', ModalResetarSenhaController)

ModalResetarSenhaController.$inject = ['$uibModalInstance', 'recursoUsuario', 'usuario']

function ModalResetarSenhaController($uibModalInstance, recursoUsuario, usuario) {
  var self = this;
  self.usuario = usuario
  self.processando = false
  self.modal = {
    titulo: 'Solicitar nova senha',
    mensagem: 'Solicitar nova senha para o usuário',
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
        recursoUsuario.renovarSenha(null, self.usuario.Email).$promise
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