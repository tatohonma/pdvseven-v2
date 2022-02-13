angular.module('AtivacaoApp')
  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/clientes', {
      templateUrl: '/clientes/index'
    })
      .when('/clientes/new', {
        templateUrl: '/clientes/edit'
      })
      .when('/clientes/edit/:id', {
        templateUrl: '/clientes/edit'
      })
      .when('/ativacoes', {
        templateUrl: '/ativacoes/index'
      })
      .when('/ativacoes/new', {
        templateUrl: '/ativacoes/edit'
      })
      .when('/ativacoes/edit/:id', {
        templateUrl: '/ativacoes/edit'
      })
      .otherwise({
        redirectTo: '/'
      })
  }])
