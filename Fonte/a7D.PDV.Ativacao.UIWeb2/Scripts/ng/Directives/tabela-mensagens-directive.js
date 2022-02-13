angular.module('AtivacaoApp').directive('tabelaMensagens', tabelaMensagens)
function tabelaMensagens () {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/tabelamensagens.html'
  }
}