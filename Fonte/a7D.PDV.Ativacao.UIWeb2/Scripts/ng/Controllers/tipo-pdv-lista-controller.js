;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .controller('TipoPdvListaController', ['$scope', 'recursoTipoPdv', 'AuthenticationService', function ($scope, recursoTipoPdv, AuthenticationService) {
      $scope.tiposPdv = []
      recursoTipoPdv.query(function (tiposPdv) {
        $scope.tiposPdv = tiposPdv
      }, function (err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        console.error(err.data.Message)
      })
    }])
})()
