angular.module('AtivacaoApp').directive('licencas', licencas)
function licencas () {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/licencas.html',
    scope: {
      ngModel: '=',
      adicionar: '&',
      remover: '&',
      pdvs: '='
    }
  }
}