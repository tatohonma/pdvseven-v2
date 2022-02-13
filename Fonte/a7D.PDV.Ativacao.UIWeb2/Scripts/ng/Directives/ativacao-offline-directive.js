angular.module('AtivacaoApp').directive('ativacaoOffline', ativacaoOffline)
ativacaoOffline.$inject = ['$timeout']
function ativacaoOffline ($timeout) {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/ativacaooffline.html',
    scope: true,
    link: function (scope, element, attr) {
      var anchor = element.children()[0]
      scope.$on('ativacao-started', function () {
        $(anchor).attr('disabled', 'disabled')
      })
      scope.$on('ativacao-err', function (event, err) {
        $(anchor).attr('title', err.Message)
      })
      scope.$on('ativacao-downloaded', function (event, ativacaoOffline) {
        scope.gerarAtivacao = function () {}
        $(anchor).removeAttr('disabled')
        $(anchor).attr('data-titulo', 'Ativação Offline')
        $(anchor).attr('data-dados', ativacaoOffline)
        $(anchor).attr('data-toggle', 'modal')
        $(anchor).attr('data-target', '#modal-info')
        $(anchor).removeClass('btn-default')
          .addClass('btn-success')
        $timeout(function () { $(anchor).trigger('click') }, 100)
      })
    },
    controller: ['$scope', '$attrs', 'recursoAtivacaoOffline', function ($scope, $attrs, recursoAtivacaoOffline) {
      $scope.gerarAtivacao = function () {
        $scope.$emit('ativacao-started')
        recursoAtivacaoOffline.get({ id: $attrs.ativacao }, function (response) {
          $scope.$emit('ativacao-downloaded', response.ativacaoOffline)
        }, function (err) {
          $scope.$emit('ativacao-err', err)
        })
      }
    }]
  }
}