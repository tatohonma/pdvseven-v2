angular.module('AtivacaoApp').directive('envioMensagens', envioMensagens)
function envioMensagens () {
  return {
    restrict: 'E',
      templateUrl: '/static/diretivas/envioMensagens.html'
  }
}