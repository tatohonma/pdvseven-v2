;(function () {
    'use strict';

    angular
        .module('AtivacaoApp')
        .controller('RevendasListaController', RevendasListaController);

    RevendasListaController.$inject = [
        '$scope', '$rootScope',
        'recursoRevendas', 'AuthenticationService', 'AccountContext'
    ];

    function RevendasListaController ($scope, $rootScope, recursoRevendas, AuthenticationService, AccountContext) {
        $scope.revendas  = [];
        $scope.revenda   = null;
        $scope.carregado = false;
        $scope.error     = false;

        function carregar() {
            if (!AccountContext.hasRole('admin')) {
                $scope.carregado = true;
                $scope.revendas  = [];
                return;
            }

            recursoRevendas.query().$promise
                .then(function (revendas) {
                    console.log(revendas);
                    $scope.revendas  = revendas || [];
                    $scope.carregado = true;
                    $scope.error     = false;
                })
                .catch(function (err) {
                    if (err && err.status === 401) {
                        AuthenticationService.ClearCredentials();
                    }
                    $scope.error     = true;
                    $scope.carregado = true;
                    console.error(err);
                });
        }

        var offLogado   = $rootScope.$on('logado', carregar);
        var offDeslogado= $rootScope.$on('deslogado', carregar);
        $scope.$on('$destroy', function () {
            offLogado && offLogado();
            offDeslogado && offDeslogado();
        });

        carregar();
    }
})();
