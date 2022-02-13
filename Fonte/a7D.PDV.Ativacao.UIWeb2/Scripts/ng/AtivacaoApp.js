;(function () {
  'use strict'
  angular
    .module('AtivacaoApp', ['ui.mask', 'ui.bootstrap', 'ngRoute', 'ngResource', 'ngCookies', 'ngTouch', 'ngTable', 'angular-jwt'])
    .config(config)
    .run(run)

  config.$inject = ['$routeProvider', '$locationProvider', 'ngTableFilterConfigProvider']
  function config ($routeProvider, $locationProvider, ngTableFilterConfigProvider) {
    $locationProvider.hashPrefix('');
    $routeProvider.when('/clientes', {
      templateUrl: '/static/clientes/index.html'
    })
      .when('/clientes/new', {
        templateUrl: '/static/clientes/edit.html'
      })
      .when('/clientes/edit/:id', {
        templateUrl: '/static/clientes/edit.html'
      })
      .when('/ativacoes', {
        reloadOnSearch: false,
        templateUrl: '/static/ativacoes/index.html'
      })
      .when('/ativacoes/new', {
        templateUrl: '/static/ativacoes/edit.html'
      })
      .when('/ativacoes/edit/:id', {
        templateUrl: '/static/ativacoes/edit.html'
      })
      .when('/mensagens', {
        templateUrl: '/static/mensagens/index.html'
      })
      .when('/usuarios/', {
          templateUrl: '/static/usuarios/index.html',
          controller: 'UsuariosController',
          controllerAs: 'vm'
      })
      .when('/usuarios/new', {
          templateUrl: '/static/usuarios/edit.html',
          controller: 'UsuarioController',
          controllerAs: 'vm'
      })
      .when('/usuarios/edit/:id', {
          templateUrl: '/static/usuarios/edit.html',
          controller: 'UsuarioController',
          controllerAs: 'vm'
      })
      .when('/cadastro/:hash', {
          templateUrl: '/static/cadastro/index.html',
          controller: 'CadastroController',
          controllerAs: 'vm'
      })
      .when('/login', {
        controller: 'LoginController',
        templateUrl: '/static/login/index.html',
        controllerAs: 'vm'
      })
      .when('/', {
        templateUrl: '/static/home/index.html'
      })

    var filterAlias = { 'checkbox': '/static/filters/checkbox.html' }

    ngTableFilterConfigProvider.setConfig({
      aliasUrls: filterAlias
    })
  }

  run.$inject = ['$rootScope', '$location', '$cookieStore', '$http']
  function run ($rootScope, $location, $cookieStore, $http) {
    // $http.defaults.useXDomain = true

    // keep user logged in after page refresh
    $rootScope.globals = $cookieStore.get('globals') || {}
    if ($rootScope.globals.currentUser) {
      $http.defaults.headers.common['x-auth-token'] = $rootScope.globals.currentUser.jwt
    }

    $rootScope.$on('$locationChangeStart', function (event, next, current) {
      // redirect to login page if not logged in and trying to access a restricted page
      var restrictedPage = $.inArray($location.path(), ['/login']) === -1
      restrictedPage = restrictedPage && $location.path().indexOf('/cadastro')
      var loggedIn = $rootScope.globals.currentUser
      if (restrictedPage && !loggedIn) {
        $location.path('/login')
      }
      if (loggedIn) {
        var adm = loggedIn.adm === true
        $rootScope.$broadcast('logado', adm)
      } else {
        $rootScope.$broadcast('deslogado')
      }
    })
  }
})()
