angular.module('AtivacaoApp')
  .controller('ModalExcluirUsuarioController', ModalExcluirUsuarioController)

ModalExcluirUsuarioController.$inject = ['$uibModalInstance', 'recursoUsuario', 'usuario']

function ModalExcluirUsuarioController($uibModalInstance, recursoUsuario, usuario) {
  var self = this;
  self.usuario = usuario
  self.processando = false
  self.modal = {
    titulo: 'Excluir Usuário',
    mensagem: 'Deseja realmente excluir o usuário',
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
        recursoUsuario.delete({ id: self.usuario.IDUsuario}).$promise
          .then(function(data) {
            $uibModalInstance.close()
          })
          .catch(function(err) {
            $uibModalInstance.dismiss(err)
          })
      },
      classe: 'btn-danger',
      texto: 'Sim'
    }
  }
}