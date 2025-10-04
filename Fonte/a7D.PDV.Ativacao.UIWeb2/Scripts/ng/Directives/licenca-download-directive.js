angular.module('AtivacaoApp').directive('licencaDownload', licencaDownload)
licencaDownload.$inject = ['$timeout', '$log'];
function licencaDownload ($timeout, $log) {
    return {
        restrict: 'E',
        templateUrl: '/static/diretivas/licencadownload.html',
        scope: {
            id: '@',          // <-- vem de id="{{ativacao.IDAtivacao}}"
            arquivo: '@'      // <-- vem de arquivo="{{...}}.lic"
        },
        link: function (scope, element) {
            var anchor = element.children()[0];

            // debug: ver o id resolvido
            scope.$watch('id', function(v){ if (v) $log.debug('[licencaDownload] id =', v); });

            scope.$on('download-start', function () {
                $(anchor).attr('disabled', 'disabled');
            });

            scope.$on('downloaded', function (event, data) {
                scope.downloadLicenca = function () {};
                $(anchor).attr({
                    href: 'data:application/octet-stream;base64,' + data,
                    download: scope.arquivo
                }).removeAttr('disabled')
                    .removeClass('btn-default')
                    .addClass('btn-success');
                $timeout(function () { $(anchor).trigger('click'); }, 300);
            });

            scope.$on('nofile', function () {
                $(anchor).removeClass('btn-default').addClass('btn-danger');
                scope.downloadLicenca = function () {};
            });
        },
        controller: ['$scope', 'recursoBaixarLicenca', '$log',
            function ($scope, recursoBaixarLicenca, $log) {

                $scope.downloadLicenca = function () {
                    var id = $scope.id; // já interpolado
                    $log.debug('[licencaDownload] GET com id =', id);
                    $scope.$emit('download-start');

                    recursoBaixarLicenca.get({ id: id }, function (resp) {
                        $log.debug('[licencaDownload] sucesso id=', id);
                        $scope.$emit('downloaded', resp.data);
                    }, function (err) {
                        $log.error('[licencaDownload] erro id=', id, err);
                        $scope.$emit('nofile');
                    });
                };
            }
        ]
    };
}
