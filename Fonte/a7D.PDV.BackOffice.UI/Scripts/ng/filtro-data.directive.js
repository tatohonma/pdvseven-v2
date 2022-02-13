angular.module('backoffice')
  .directive('filtroData', filtroData);

function filtroData() {
  return {
    restrict: 'E',
    scope: {
      acoes: '=',
      atual: '@'
    },
    template: '<div class="dropdown">' +
                '<button class="btn btn-default dropdown-toggle" data-toggle="dropdown">Filtro ({{ atual }})&nbsp;<div class="caret"></div></button>' +
                '<ul class="dropdown-menu">' +
                '<li ng-repeat="acao in acoes"><a href="#" ng-click="acao.filtro()">{{ acao.nome }}</a></li>' +
                '</ul>' +
              '</div>',
    link: function(scope, elem, attr) {
      scope.atual = "Últimos 30 dias";
      elem.on('click', function() {
        elem.unbind('click');
        elem.find('ul').children().find('a').on('click', function() {
          scope.atual = $(this).text();
        })
      })
    }
  }
}