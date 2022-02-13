angular.module('AtivacaoApp')
  .directive('ativacaoOfflineE', ativacaoOfflineE);
ativacaoOfflineE.$inject = ['$timeout', '$uibModal']
function ativacaoOfflineE($timeout, $uibModal) {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/ativacaoofflinee.html',
    scope: {
      dados: '@',
      dadosObtidos: '@',
      erro: '@', 
      ativacao: '=',
      buscando: '@',
    }, 
    link: function(scope, element, attr) {
      var anchor = element
      scope.buscando = false
      scope.dadosObtidos = false
    }, 
    controller: ['$scope', 'recursoAtivacaoOffline', 'modalErro', function($scope, recursoAtivacaoOffline, modalErro) {
      $scope.gerarAtivacaoOffline = function() {
        $scope.buscando = true
        recursoAtivacaoOffline.get({ id: $scope.ativacao.ChaveAtivacao }, function (response) {
          $scope.buscando = false
          $scope.dadosObtidos = true
          $scope.dados = response.ativacaoOffline
        }, function (err) {
          $scope.buscando = false
          modalErro(err)
        })
      }
    }]
  }
}