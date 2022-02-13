angular.module('backoffice')
  .directive('caixaDiferenca', caixaDiferenca);

function caixaDiferenca() {
  return {
    restrict: 'E',
    scope: {
      valor: '=valor'
    },
    template: '<span ng-hide="bateu">({{ texto }} {{ valor | reais }})</span>',
    link: function(scope, elem, attrs) {
      scope.bateu = scope.valor === 0;
      if (scope.valor < 0) {
        scope.texto = 'Faltou';
        angular.element(elem).addClass('text-danger');
      } else {
        scope.texto = 'Sobrou';
      }
    }
  }
}