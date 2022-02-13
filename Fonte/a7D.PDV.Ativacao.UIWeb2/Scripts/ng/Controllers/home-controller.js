;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .controller('HomeController', HomeController)

  HomeController.$inject = ['$location', 'AuthenticationService']
  function HomeController ($location, AuthenticationService) {
    var vm = this
    vm.deslogar = function () {
      AuthenticationService.ClearCredentials()
      $location.path('/login')
    }
  }
})()
