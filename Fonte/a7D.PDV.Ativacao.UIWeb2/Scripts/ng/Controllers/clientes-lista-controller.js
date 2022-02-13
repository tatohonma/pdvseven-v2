;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .controller('ClientesListaController', ['$scope', 'recursoClientes', 'AuthenticationService', function ($scope, recursoClientes, AuthenticationService) {
      $scope.clientes = []
      $scope.carregado = false
      recursoClientes.query({pagina: -1, busca: '', quantidade: -1}, function (data) {
        $scope.carregado = true
        $scope.clientes = data.clientes
      }, function (err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        $scope.error = true
        console.error(err.data.Message)
      })
    }])
})()
