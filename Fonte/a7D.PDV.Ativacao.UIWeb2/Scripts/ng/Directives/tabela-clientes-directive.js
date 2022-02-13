angular.module('AtivacaoApp').directive('tabelaClientes', tabelaClientes)
function tabelaClientes () {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/tabelaclientes.html'
  }
}