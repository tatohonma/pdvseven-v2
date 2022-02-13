angular.module('backoffice')
  .controller('fechamento', fechamento);

fechamento.$inject = ["$window", "$scope"]

function fechamento($window, $scope) {
  var vm = this;

  var backoffice = $window.backoffice;

  vm.fechamento = {};
  vm.carregado = false;
  vm.semFechamento = false;

  vm.filtrar = function(id) {
    vm.carregado = false;
    backoffice.obterFechamento(id)
      .then(vm.atualizarDados)
      .catch(function (err) {
        vm.carregado = true;
        console.error(err);
      })
  }

  vm.atualizarDados = function(strData) {
    var data = angular.fromJson(strData);
    vm.fechamento = data;
    vm.carregado = true;
    vm.semFechamento = data.semFechamento;
    $scope.$apply();
  }
}