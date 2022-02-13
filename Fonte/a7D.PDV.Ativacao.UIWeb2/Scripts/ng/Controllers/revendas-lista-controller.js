;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .controller('RevendasListaController', ['$scope', '$rootScope', 'recursoRevendas', 'AuthenticationService', function ($scope, $rootScope, recursoRevendas, AuthenticationService) {
      
      var adm = $rootScope.globals.currentUser.adm === true

      if(!adm) {
        return
      }

      $scope.revendas = []
      $scope.error = false
      $scope.carregado = false
      $scope.revenda = {}
      recursoRevendas.query(function (revendas) {
        $scope.carregado = true
        $scope.revendas = revendas
      }, function (err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        $scope.error = true
        console.error(err)
      })
    }
    ])
})()
