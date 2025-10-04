;(function () {
    'use strict'
    // const address = 'http://apipdvseven.azurewebsites.net'
    // const address = 'http://localhost:5278'
    const address = 'https://localhost:7060'

    const AtivacaoApp = angular.module('AtivacaoApp')

    AtivacaoApp.factory('_', _)
    _.$inject = ['$window']
    function _ ($window) { return $window._ }

    AtivacaoApp.factory('modalErro', ['$uibModal', function($uibModal) {
        return function(erro) {
            $uibModal.open({
                templateUrl: 'static/template/modal-erro-template.html',
                controller: 'ModalErroController',
                controllerAs: 'vm',
                resolve: { erro: function() { return erro } }
            })
        }
    }])

    AtivacaoApp.factory('makeCancelable',[ '$q', function($q) {
        return function(promise) {
            var hasCanceled_ = false
            var wrappedPromise = $q(function(resolve, reject) {
                promise.then(function (val) {
                    hasCanceled_ ? reject({ isCancelled: true }) : resolve(val)
                })
                promise.catch(function(err) {
                    hasCanceled_ ? reject({ isCancelled: true}) : reject(err)
                })
            })
            return { promise: wrappedPromise, cancel: function(){ hasCanceled_ = true } }
        }
    }])

    // -------------------- EMAILS --------------------
    AtivacaoApp.factory('recursoEmail', recursoEmail)
    recursoEmail.$inject = ['$resource']
    function recursoEmail($resource) {
        // /api/emails/valid?email=&id=
        return $resource(address + '/api/emails/valid', null, {
            existe: {
                method: 'GET',
                isArray: false,
                params: { email: '@email', id: '@id' },
                transformResponse: function (data, headersGetter, status) {
                    // 200 livre / 409 já existe (o controller deve retornar 409 quando conflitar)
                    return status === 200
                }
            }
        })
    }

    // -------------------- USER --------------------
    AtivacaoApp.factory('recursoUsuario', recursoUsuario)
    recursoUsuario.$inject = ['$resource']
    function recursoUsuario($resource) {
        // rotas novas: /api/user, /api/user/{id}, /api/user/renovar, /api/user/reenviar, /api/user/password/reset, /api/user/generate-reset-token
        return $resource(address + '/api/user/:id', null, {
            query: {
                method: 'GET',
                // a API nova espera Name, Email, Active, PendingRegistration, Admin, Deleted
                // mantemos page/count
                params: { page: '@page', count: '@count' },
                isArray: true
            },
            get:  { method: 'GET', isArray: false },
            delete: { method: 'DELETE' },
            save: {
                method: 'POST',
                transformResponse: function(data, headers) {
                    return { data: data, headers: headers() }
                }
            },
            update: { method: 'PUT' },

            renovarSenha: {
                method: 'POST',
                url: address + '/api/user/renovar',
                headers: { 'Content-Type' : 'text/plain' } // body = "email"
            },
            reenviarEmail: {
                method: 'POST',
                url: address + '/api/user/reenviar',
                headers: { 'Content-Type' : 'text/plain' } // body = "email"
            },
            gerarResetToken: {
                method: 'POST',
                url: address + '/api/user/generate-reset-token',
                headers: { 'Content-Type' : 'text/plain' } // body = "email"
            },
            resetPassword: {
                method: 'POST',
                url: address + '/api/user/password/reset' // body = { userId, token, newPassword, newName }
            }
        })
    }

    AtivacaoApp.factory('cadastroUsuario', cadastroUsuario)
    cadastroUsuario.$inject = ['recursoUsuario', '$q']
    function cadastroUsuario(recursoUsuario, $q) {
        return {
            salvar: function(usuario) {
                return $q(function (resolve, reject) {
                    // novos DTOs usam "id"
                    if (usuario.id) {
                        recursoUsuario.update({ id: usuario.id }, usuario,
                            function(data) { resolve({ data: data, save: false }) },
                            function(err) { reject(err) })
                    } else {
                        recursoUsuario.save(usuario,
                            function(resp) { resolve({ data: angular.fromJson(resp.data), save: true }) },
                            function(err) { reject(err) })
                    }
                })
            }
        }
    }

    // -------------------- VALIDATION (antiga Validade) --------------------
    AtivacaoApp.factory('recursoValidade', recursoValidade)
    recursoValidade.$inject = ['$resource']
    function recursoValidade($resource) {
        // POST /api/validation/liberacao/{id}
        return $resource(address + '/api/validation/liberacao/:id', null, {
            liberacao: { method: 'POST' }
        })
    }

    // -------------------- ACTIVATION (backoffice) --------------------
    AtivacaoApp.factory('recursoAtivacao', recursoAtivacao)
    recursoAtivacao.$inject = ['$resource']
    function recursoAtivacao ($resource) {
        // /api/activation  e  /api/activation/{id}
        return $resource(address + '/api/activation/:id', null, {
            update: { method: 'PUT' },
            save: {
                method: 'POST',
                transformResponse: function (data, headers) {
                    return { data: data, headers: headers() }
                }
            },
            query: {
                method: 'GET',
                params: { page: '@page', count: '@count' },
                isArray: true
            },
            get: { method: 'GET', isArray: false }
        })
    }

    // -------------------- MENSAGENS --------------------
    AtivacaoApp.factory('recursoMensagens', recursoMensagens)
    recursoMensagens.$inject = ['$resource']
    function recursoMensagens($resource) {
        // /api/mensagens (GET/POST), /api/mensagens/receber?chave=, /api/mensagens/syncmsg?... (os 2 últimos você chama direto via $http se precisar)
        return $resource(address + '/api/mensagens/:id', null, {
            query: {
                method: 'GET',
                params: { page: '@page', count: '@count' },
                isArray: true
            },
            message: { method: 'POST' }
        })
    }

    // -------------------- GENERATE ACTIVATION KEY --------------------
    AtivacaoApp.factory('recursoGerarChaveAtivacao', recursoGerarChaveAtivacao)
    recursoGerarChaveAtivacao.$inject = ['$resource']
    function recursoGerarChaveAtivacao ($resource) {
        // GET /api/generate-activation-key?resellerId=123
        return $resource(address + '/api/generate-activation-key', null, {
            get: { method: 'GET', isArray: false }
        })
    }
    // -------------------- CLIENTES --------------------
    AtivacaoApp.factory('recursoClientes', recursoClientes)
    recursoClientes.$inject = ['$resource']
    function recursoClientes ($resource) {
        // atenção: rota com C maiúsculo: /api/clientes
        return $resource(address + '/api/client/:id', null, {
            update: { method: 'PUT' },
            query: {
                method: 'GET',
                params: { page: '@page', count: '@count' },
                isArray: true
            },
            get: { method: 'GET', isArray: false },
            save: {
                method: 'POST',
                transformResponse: function (data, headers) {
                    return { data: data, headers: headers() }
                }
            }
        })
    }

    // -------------------- RESELLERS (grafia nova: ressellers) --------------------
    AtivacaoApp.factory('recursoRevendas', recursoRevendas)
    recursoRevendas.$inject = ['$resource']
    function recursoRevendas ($resource) {
        // OpenAPI: /api/ressellers
        return $resource(address + '/api/ressellers/:id', null, {
            update: { method: 'PUT' }
        })
    }

    // -------------------- PDV TYPE --------------------
    AtivacaoApp.factory('recursoTipoPdv', recursoTipoPdv)
    recursoTipoPdv.$inject = ['$resource']
    function recursoTipoPdv ($resource) {
        // OpenAPI: /api/pdv-type  e  /api/pdv-type/{id}
        return $resource(address + '/api/pdv-type/:id', null, {
            update: { method: 'PUT' }
        })
    }

    // -------------------- HELPERS DE CADASTRO --------------------
    AtivacaoApp.factory('cadastroAtivacao', cadastroAtivacao)
    cadastroAtivacao.$inject = ['recursoAtivacao', '$q']
    function cadastroAtivacao (recursoAtivacao, $q) {
        return {
            salvar: function (ativacao) {
                return $q(function (resolve, reject) {
                    if (ativacao.id) {
                        recursoAtivacao.update({ id: ativacao.id }, ativacao,
                            function (data) { resolve({ data: data, save: false }) },
                            function (err) { reject(err) })
                    } else {
                        recursoAtivacao.save(ativacao,
                            function (resp) { resolve({ data: JSON.parse(resp.data), save: true }) },
                            function (err) { reject(err) })
                    }
                })
            }
        }
    }

    AtivacaoApp.factory('cadastroCliente', cadastroCliente)
    cadastroCliente.$inject = ['recursoClientes', '$q']
    function cadastroCliente (recursoClientes, $q) {
        return {
            salvar: function (cliente) {
                return $q(function (resolve, reject) {
                    if (cliente.id) {
                        recursoClientes.update({ id: cliente.id }, cliente,
                            function () { resolve() },
                            function (err) { reject(err) })
                    } else {
                        recursoClientes.save(cliente,
                            function (resp) { resolve(JSON.parse(resp.data).id) },
                            function (err) { reject(err) })
                    }
                })
            }
        }
    }

    // -------------------- OFFLINE VALIDATION --------------------
    AtivacaoApp.factory('recursoAtivacaoOffline', recursoAtivacaoOffline)
    recursoAtivacaoOffline.$inject = ['$resource']
    function recursoAtivacaoOffline ($resource) {
        // OpenAPI: /api/offline-validation?id=XXXX (query, não path)
        return $resource(address + '/api/offline-validation', null, {
            get: { method: 'GET', isArray: false, params: { id: '@id' } }
        })
    }

    // -------------------- DOWNLOAD LICENSE --------------------
    AtivacaoApp.factory('recursoBaixarLicenca', recursoBaixarLicenca)
    recursoBaixarLicenca.$inject = ['$resource']
    function recursoBaixarLicenca ($resource) {
        // OpenAPI: /api/download-license/{id}
        return $resource(address + '/api/download-license/:id', null, {
            get: {
                method: 'GET',
                isArray: false,
                transformResponse: function (data) { return { data: data } }
            }
        })
    }

    // -------------------- AUTH --------------------
    AtivacaoApp.factory('AuthenticationService', AuthenticationService)
    AuthenticationService.$inject = ['$http', '$cookieStore', '$rootScope', '$window']
    function AuthenticationService ($http, $cookieStore, $rootScope, $window) {
        var service = {}
        service.Login = Login
        service.SetCredentials = SetCredentials
        service.ClearCredentials = ClearCredentials
        return service

        function parseJwt (token) {
            try {
                var base64Url = token.split('.')[1]
                var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
                var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
                    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
                }).join(''))
                return JSON.parse(jsonPayload)
            } catch { return {} }
        }

        function extractRolesAndPerms (token, loginPayload) {
            var claims = token ? parseJwt(token) : {}
            var roles = claims.roles || claims.role || claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []
            console.log(roles)
            
            if (typeof roles === 'string') roles = [roles]
            var perms = claims.permissions || claims.perms || []
            if (typeof perms === 'string') perms = [perms]

            if ((!roles || roles.length === 0) && loginPayload && loginPayload.user && Array.isArray(loginPayload.user.roles))
                roles = loginPayload.user.roles
            if ((!perms || perms.length === 0) && loginPayload && loginPayload.user && Array.isArray(loginPayload.user.permissions))
                perms = loginPayload.user.permissions

            roles = (roles || []).map(String)
            perms = (perms || []).map(String)
            return { roles, perms }
        }

        function Login (username, password, callback) {
            $http.post(address + '/api/auth/login', { email: username, password: password })
                .then(function (response) {
                    const data = response.data || {}
                    const token = data.accessToken || data.token || data.jwt
                    const { roles, perms } = extractRolesAndPerms(token, data)
                    callback({ success: true, contents: { token, roles, perms, raw: data } })
                }, function (response) {
                    callback({ success: false, contents: response.data })
                })
        }

        function SetCredentials (username, password, token, isAdminOrRoles, maybePerms) {
            
            var roles = Array.isArray(isAdminOrRoles)
                ? isAdminOrRoles
                : (isAdminOrRoles === true ? ['admin'] : []);

            var perms = Array.isArray(maybePerms) ? maybePerms : [];

            

            $rootScope.globals = {
                currentUser: { username, token, roles, permissions: perms }
            };

            $http.defaults.headers.common['Authorization'] = token ? ('Bearer ' + token) : null;
            $cookieStore.put('globals', $rootScope.globals);

            $rootScope.$broadcast('logado', roles.includes('admin'));
            $window.location.href = '#/ativacoes';
        }

        function ClearCredentials () {
            $rootScope.globals = {}
            $cookieStore.remove('globals')
            delete $http.defaults.headers.common['Authorization']
            $rootScope.$broadcast('deslogado')
            $window.location.href = '#/login'
        }
    }

    // -------------------- Base64 helper --------------------
    const Base64 = {
        keyStr: 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=',
        encode: function (input) {
            var output = '', i = 0, chr1, chr2, chr3, enc1, enc2, enc3, enc4
            do {
                chr1 = input.charCodeAt(i++)
                chr2 = input.charCodeAt(i++)
                chr3 = input.charCodeAt(i++)
                enc1 = chr1 >> 2
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4)
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6)
                enc4 = chr3 & 63
                if (isNaN(chr2)) { enc3 = enc4 = 64 } else if (isNaN(chr3)) { enc4 = 64 }
                output += this.keyStr.charAt(enc1) + this.keyStr.charAt(enc2) + this.keyStr.charAt(enc3) + this.keyStr.charAt(enc4)
            } while (i < input.length)
            return output
        },
        decode: function (input) {
            var output = '', i = 0, chr1, chr2, chr3, enc1, enc2, enc3, enc4
            var base64test = /[^A-Za-z0-9\+\/\=]/g
            if (base64test.exec(input)) {
                window.alert('There were invalid base64 characters in the input text.\n' +
                    "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                    'Expect errors in decoding.')
            }
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, '')
            do {
                enc1 = this.keyStr.indexOf(input.charAt(i++))
                enc2 = this.keyStr.indexOf(input.charAt(i++))
                enc3 = this.keyStr.indexOf(input.charAt(i++))
                enc4 = this.keyStr.indexOf(input.charAt(i++))
                chr1 = (enc1 << 2) | (enc2 >> 4)
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2)
                chr3 = ((enc3 & 3) << 6) | enc4
                output += String.fromCharCode(chr1)
                if (enc3 !== 64) output += String.fromCharCode(chr2)
                if (enc4 !== 64) output += String.fromCharCode(chr3)
            } while (i < input.length)
            return output
        }
    }
    
    
})()
