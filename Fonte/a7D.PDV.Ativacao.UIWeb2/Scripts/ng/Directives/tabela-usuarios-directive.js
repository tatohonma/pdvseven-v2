angular.module('AtivacaoApp').directive('tabelaUsuarios', tabelaUsuarios)
function tabelaUsuarios() {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/tabelausuarios.html'
  }
}