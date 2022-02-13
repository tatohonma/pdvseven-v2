angular.module('AtivacaoApp').directive('tabelaAtivacoes', tabelaAtivacoes)
function tabelaAtivacoes () {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/tabelaativacoes.html'
  }
}