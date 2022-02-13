angular.module('AtivacaoApp').directive('licencaDownload', licencaDownload)
licencaDownload.$inject = ['$timeout']
function licencaDownload ($timeout) {
  return {
    restrict: 'E',
    templateUrl: '/static/diretivas/licencadownload.html',
    scope: true,
    link: function (scope, element, attr) {
      var anchor = element.children()[0]

      scope.$on('download-start', function () {
        $(anchor).attr('disabled', 'disabled')
      })

      scope.$on('downloaded', function (event, data) {
        scope.downloadLicenca = function () {}
        $(anchor).attr({
          href: 'data:application/octet-stream;base64,' + data,
          download: attr.arquivo
        }).removeAttr('disabled')
          .removeClass('btn-default')
          .addClass('btn-success')
        $timeout(function () {
          $(anchor).trigger('click')
        }, 500)
      })

      scope.$on('nofile', function () {
        $(anchor).removeClass('btn-default')
          .addClass('btn-danger')

        scope.downloadLicenca = function () {}
      })
    },
    controller: ['$scope', '$attrs', 'recursoBaixarLicenca', '$http', function ($scope, $attrs, recursoBaixarLicenca, $http) {
      $scope.downloadLicenca = function () {
        $scope.$emit('download-start')
        recursoBaixarLicenca.get({ id: $attrs.id }, function (response) {
          $scope.$emit('downloaded', response.data)
        }, function () {
          $scope.$emit('nofile')
        })
      }
    }]
  }
}