angular.module('AtivacaoApp')
  .directive('liberarTempoariamente', liberarTempoariamente)

liberarTempoariamente.$inject = []

function liberarTempoariamente() {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/liberarTemporariamente.html',
    scope: {
      ativacao: '=',
      buscando: '@',
      mostrarReativar: '@'
    },
    link: function(scope, element, attrs) {
      scope.buscando = false
      scope.mostrarReativar = scope.ativacao.ReativadoSuporte === false && scope.ativacao.Ativo === false
    },
    controller: ['$scope', 'modalErro', 'recursoValidade', function($scope, modalErro, recursoValidade) {
      $scope.liberar = function() {
        $scope.buscando = true
        recursoValidade.liberacao({id: $scope.ativacao.IDAtivacao}, null, function(validade) {
          $scope.buscando = false
          $scope.ativacao.ReativadoSuporte = true
          $scope.ativacao.DataValidadeProvisoria = validade.Validade
          $scope.mostrarReativar = $scope.ativacao.ReativadoSuporte === false && $scope.ativacao.Ativo === false
        }, function(err) {
          $scope.buscando = false
          modalErro(err)
        })
      }
    }]
  }
}