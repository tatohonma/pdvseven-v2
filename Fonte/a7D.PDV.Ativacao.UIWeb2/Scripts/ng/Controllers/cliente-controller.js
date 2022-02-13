;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .controller('ClienteController', ClienteController)

  ClienteController.$inject = ['$scope', '$location', '$routeParams', '$timeout', 'recursoClientes', 'cadastroCliente', 'AuthenticationService']
  function ClienteController ($scope, $location, $routeParams, $timeout, recursoClientes, cadastroCliente, AuthenticationService) {
    $scope.carregado = false
    $scope.cliente = {}
    $scope.mensagem = null
    $scope.mensagemErr = null
    $scope.doc = 'CNPJ'
    if ($routeParams.id) {
      recursoClientes.get({ id: $routeParams.id }, function (cliente) {
        $scope.carregado = true
        $scope.cliente = cliente
        $scope.doc = cliente.CNPJCPF && cliente.CNPJCPF.toString().length === 14 ? 'CNPJ' : 'CPF'
      }, function (err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        $scope.carregado = true
        $scope.messageErr = err.data.Message
      })
    } else {
      $scope.cliente.IDCliente = 0
      $scope.carregado = true
    }

    $scope.salvar = function (cliente) {
      cadastroCliente.salvar(cliente).then(function (id) {
        $scope.mensagem = 'Salvo com sucesso!'
        if (id) {
          $scope.carregado = false
          $timeout(function () {
            $location.path('/clientes/edit/' + id)
          }, 1500)
        }
      }).catch(function (err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        $scope.mensagemErr = err.data.Message
      })
    }
  }
})()
