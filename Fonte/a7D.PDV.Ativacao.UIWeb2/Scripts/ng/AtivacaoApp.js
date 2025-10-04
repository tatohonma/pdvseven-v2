;(function () {
    'use strict'

    angular
        .module('AtivacaoApp', [
            'ui.mask', 'ui.bootstrap', 'ngRoute', 'ngResource',
            'ngCookies', 'ngTouch', 'ngTable', 'angular-jwt'
        ])
        .config(config)
        .factory('AccountContext', AccountContextFactory)
        .directive('hasAccess', hasAccessDirective)
        .run(run)

    // ---------------- CONFIG ----------------
    config.$inject = ['$routeProvider', '$locationProvider', 'ngTableFilterConfigProvider']
    function config ($routeProvider, $locationProvider, ngTableFilterConfigProvider) {
        $locationProvider.hashPrefix('')

        $routeProvider
            .when('/clientes', {
                templateUrl: '/static/clientes/index.html',
                data: { roles: ['admin'] }
            })
            .when('/clientes/new', {
                templateUrl: '/static/clientes/edit.html',
                data: { roles: ['admin'] }
            })
            .when('/clientes/edit/:id', {
                templateUrl: '/static/clientes/edit.html',
                data: { roles: ['admin'] }
            })
            .when('/ativacoes', {
                reloadOnSearch: false,
                templateUrl: '/static/ativacoes/index.html'
            })
            .when('/ativacoes/new', {
                templateUrl: '/static/ativacoes/edit.html',
                data: { roles: ['admin'] }
            })
            .when('/ativacoes/edit/:id', {
                templateUrl: '/static/ativacoes/edit.html',
                controller: 'AtivacaoController',
                controllerAs: 'vm',
                data: { roles: ['admin'] }
            })
            .when('/mensagens', {
                templateUrl: '/static/mensagens/index.html',
                data: { roles: ['admin'] }
            })
            .when('/usuarios/', {
                templateUrl: '/static/usuarios/index.html',
                controller: 'UsuariosController',
                controllerAs: 'vm',
                data: { roles: ['admin'] }
            })
            .when('/usuarios/new', {
                templateUrl: '/static/usuarios/edit.html',
                controller: 'UsuarioController',
                controllerAs: 'vm',
                data: { roles: ['admin'] }
            })
            .when('/usuarios/edit/:id', {
                templateUrl: '/static/usuarios/edit.html',
                controller: 'UsuarioController',
                controllerAs: 'vm',
                data: { roles: ['admin'] }
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
            .otherwise({ redirectTo: '/' })

        ngTableFilterConfigProvider.setConfig({
            aliasUrls: { checkbox: '/static/filters/checkbox.html' }
        })
    }
    
    

    // ---------------- AccountContext ----------------
    AccountContextFactory.$inject = ['$rootScope', '$cookieStore']
    function AccountContextFactory ($rootScope, $cookieStore) {
        function ensureSession () {
            if (!$rootScope.globals || !$rootScope.globals.currentUser) {
                $rootScope.globals = $cookieStore.get('globals') || {};
            }
            return $rootScope.globals.currentUser || null;
        }
        
        function isAuthenticated () {
            var s = ensureSession()
            return !!(s && s.token)
        }

        function roles () {
            var s = ensureSession();
            return (s && Array.isArray(s.roles)) ? s.roles.map(function(r){ return String(r).toLowerCase(); }) : [];
        }

        function permissions () {
            var s = ensureSession();
            return (s && Array.isArray(s.permissions)) ? s.permissions : [];
        }

        function hasRole (role) {
            return roles().indexOf(String(role).toLowerCase()) >= 0;
        }
        function hasAnyRole (required) {
            if (!required || required.length === 0) return true;
            var req = required.map(function(r){ return String(r).toLowerCase(); });
            var r = roles();
            for (var i=0;i<req.length;i++) if (r.indexOf(req[i]) >= 0) return true;
            return false;
        }
        
        function hasPermission (perm) {
            return permissions().indexOf(perm) >= 0
        }

        return { roles, permissions, hasRole, hasAnyRole,
            isAuthenticated: function(){ var s=ensureSession(); return !!(s && s.token); } };
    }

    // ---------------- Diretiva has-access ----------------
    // Uso:
    // <li has-access roles="Admin,Gestor">...</li>  // mostra se autenticado com qualquer um desses roles
    // <li has-access>...</li>                       // mostra se autenticado
    // <li has-access invert>...</li>                // mostra se NÃO autenticado
    hasAccessDirective.$inject = ['AccountContext']
    function hasAccessDirective (AccountContext) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                function update () {
                    var invert = attrs.invert !== undefined;
                    var requiredRoles = (attrs.roles || '')
                        .split(',')
                        .map(function (s) { return s.trim().toLowerCase(); })
                        .filter(Boolean);

                    var visible;
                    if (invert) visible = !AccountContext.isAuthenticated();
                    else if (requiredRoles.length) visible = AccountContext.isAuthenticated() && AccountContext.hasAnyRole(requiredRoles);
                    else visible = AccountContext.isAuthenticated();

                    visible ? element.show() : element.hide();
                }
                scope.$on('logado', update);
                scope.$on('deslogado', update);
                update();
            }
        };
    }

    run.$inject = ['$rootScope', '$cookieStore', '$http', '$location', '$route', 'AccountContext']
    function run ($rootScope, $cookieStore, $http, $location, $route, AccountContext) {
        $rootScope.globals = $cookieStore.get('globals') || {}
        if ($rootScope.globals.currentUser && $rootScope.globals.currentUser.token) {
            $http.defaults.headers.common['Authorization'] = 'Bearer ' + $rootScope.globals.currentUser.token
            $rootScope.$broadcast('logado', AccountContext.hasRole('admin'))
        } else {
            $rootScope.$broadcast('deslogado')
        }
        
        

        $rootScope.$on('$routeChangeStart', function (evt, next /*, current */) {
            var path = ($location.path() || '/')
            var isLogin = path === '/login'
            var isCadastro = path.indexOf('/cadastro') === 0
            var loggedIn = AccountContext.isAuthenticated()

            if (!isLogin && !isCadastro && !loggedIn) {
                evt.preventDefault()
                return $location.path('/login')
            }

            var requiredRoles = (next && next.data && next.data.roles) || []
            if (requiredRoles.length && !AccountContext.hasAnyRole(requiredRoles)) {
                evt.preventDefault()
                return $location.path('/')
            }

            if (loggedIn) $rootScope.$broadcast('logado', AccountContext.hasRole('admin'))
            else $rootScope.$broadcast('deslogado')
        })

        $rootScope.$on('$locationChangeStart', function (e, newUrl, oldUrl) {
            console.log('[loc:start]', newUrl, 'from', oldUrl);
        });
        $rootScope.$on('$routeChangeStart', function (e, next, current) {
            console.log('[route:start]', next && next.originalPath, next && next.params);
        });
        $rootScope.$on('$routeChangeSuccess', function (e, current, previous) {
            console.log('[route:ok]', current && current.originalPath, current && current.params);
        });
        $rootScope.$on('$routeChangeError', function (e, current, previous, rejection) {
            console.warn('[route:error]', current && current.originalPath, rejection);
        });
    }
})()
