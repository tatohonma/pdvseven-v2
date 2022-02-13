angular.module('AtivacaoApp').directive('mostrarLogado', mostrarLogado)
function mostrarLogado () {
  return {
    restrict: 'A',
    scope: {
      mostrar: '=',
      admOnly: '='
    },
    link: function (scope, element) {
      scope.$on('logado', function (event, adm) {
        if (scope.mostrar === true) {
          if(scope.admOnly === true) {
            if(adm == true) {
              element.show()
            } else {
              element.hide()
            }
          } else {
            element.show()
          }
        } else {
          element.hide()
        }
      })
      scope.$on('deslogado', function () {
        if (scope.mostrar === true) {
          element.hide()
        } else {
          element.show()
        }
      })
    }
  }
}