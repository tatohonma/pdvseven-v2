angular.module('AtivacaoApp')
  .controller('ModalEsqueciSenhaController', ModalEsqueciSenhaController)

ModalEsqueciSenhaController.$inject = ['$uibModalInstance', 'recursoUsuario', 'email']

function ModalEsqueciSenhaController($uibModalInstance, recursoUsuario, email) {
  var self = this;
  self.processando = false
  self.sucesso = false
  self.email = email || ''
  self.valido = false
  
  self.ok = function() {
    $uibModalInstance.close()
  }

  self.modal = {
    titulo: 'Esqueci minha senha',
    fechar: function() {
      $uibModalInstance.close()
    },
    negativo: {
      acao: function() {
        $uibModalInstance.close()
      },
      classe: 'btn-default',
      texto: 'Cancelar'
    }, 
    positivo: {
      acao: function() {
        self.processando = true
        recursoUsuario.renovarSenha(null, self.email).$promise
          .then(function(data) {
            self.sucesso = true
          })
          .catch(function(err) {
            $uibModalInstance.dismiss(err)
          })
      },
      classe: 'btn-primary',
      texto: 'Recuperar Senha'
    }
  }
}