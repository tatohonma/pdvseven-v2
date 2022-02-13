;(function () {
  'use strict'
    const address = 'http://apipdvseven.azurewebsites.net'
    // const address = 'http://localhost:7700'
  const AtivacaoApp = angular.module('AtivacaoApp')

  AtivacaoApp.factory('_', _)
  _.$inject = ['$window']
  function _ ($window) {
    return $window._
  }

  AtivacaoApp.factory('modalErro', ['$uibModal', function($uibModal) {
    return function(erro) {
      $uibModal.open({
        templateUrl: 'static/template/modal-erro-template.html', 
        controller: 'ModalErroController',
        controllerAs: 'vm',
        resolve: {
          erro: function() { return erro }
        }
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

      return {
        promise: wrappedPromise,
        cancel: function() {
          hasCanceled_ = true
        }
      }
    }
  }])

  AtivacaoApp.factory('recursoEmail', recursoEmail)
  recursoEmail.$inject = ['$resource']
  function recursoEmail($resource) {
    return $resource(address + '/api/emails', 
      null,
      {
        existe: {
          method: 'GET', 
          isArray: false,
          params: { email: '@email', id: '@id' },
          transformResponse: function(data, headers, resp) {
            console.log(resp)
            return resp === 200
          }
        }
      })
  }

  AtivacaoApp.factory('recursoUsuario', recursoUsuario)
  recursoUsuario.$inject = ['$resource']
  function recursoUsuario($resource) {
    return $resource(address + '/api/usuarios/:id',
      null,
      {
        obterPorHash: {
          method: 'GET',
          url: address + '/api/usuarios/hash/:hash',
          isArray: false
        },
        alterarSenhaHash: {
          method: 'POST',
          url: address + '/api/usuarios/hash/:hash',
        },
        query: {
          method: 'GET',
          params: { pagina: '@page', quantidade: '@count', filter: '@filter' },
          isArray: true
        },
        get: {
          method: 'GET',
          isArray: false
        },
        delete: {
          method: 'DELETE',
        },
        renovarSenha: {
          method: 'POST', 
          url: address + '/api/usuarios/renovar/',
          headers: {
            'Content-Type' : 'text/plain'
          }
        },
        reenviarEmail: {
          method: 'POST', 
          url: address + '/api/usuarios/reenviar/:email',
          headers: {
            'Content-Type' : 'text/plain'
          }
        },
        save: {
          method: 'POST',
          transformResponse: function(data, headers) {
            var response = {}
            response.data = data
            response.headers = headers()
            return response
          }
        },
        update: {
          method: 'PUT'
        }
      })
  }

  AtivacaoApp.factory('cadastroUsuario', cadastroUsuario)
  cadastroUsuario.$inject = ['recursoUsuario', '$q']
  function cadastroUsuario(recursoUsuario, $q) {
    return {
      salvar: function(usuario) {
        return $q(function (resolve, reject) {
          if(usuario.IDUsuario) {
            recursoUsuario.update({id: usuario.IDUsuario}, usuario, function(data) {
              resolve({data: data, save: false})
            }, function(err) {
              reject(err)
            })
          } else {
            recursoUsuario.save(usuario, function(data) {
              resolve({data: angular.fromJson(data.data), save: true})
            }, function(err) {
              reject(err)
            })
          }
        })
      }
    }
  }

  AtivacaoApp.factory('recursoValidade', recursoValidade)
  recursoValidade.$inject = ['$resource']
  function recursoValidade($resource) {
    return $resource(address + '/api/validade/liberacao/:id',
      null,
      {
        liberacao: {
          method: 'POST'
        }
      })
  }

  AtivacaoApp.factory('recursoAtivacao', recursoAtivacao)
  recursoAtivacao.$inject = ['$resource']
  function recursoAtivacao ($resource) {
    return $resource(address + '/api/ativacoes/:id',
      null,
      {
        update: {
          method: 'PUT'
        },
        save: {
          method: 'POST',
          transformResponse: function (data, headers) {
            var response = {}
            response.data = data
            response.headers = headers()
            return response
          }
        },
        query: {
          method: 'GET',
          params: { pagina: '@page', quantidade: '@count', filter: '@filter' },
          isArray: true
        },
        get: {
          method: 'GET',
          isArray: false,
          params: { busca: '@busca' }
        }
      })
    }

    AtivacaoApp.factory('recursoMensagens', recursoMensagens)
    recursoMensagens.$inject = ['$resource']
    function recursoMensagens($resource) {
        return $resource(address + '/api/mensagens/:id',
            null,
            {
                query: {
                    method: 'GET',
                    params: { pagina: '@page', quantidade: '@count' },
                    isArray: true
                },
                message: {
                    method: 'POST',
                },
            })
    }

  AtivacaoApp.factory('recursoGerarChaveAtivacao', recursoGerarChaveAtivacao)
  recursoGerarChaveAtivacao.$inject = ['$resource']
  function recursoGerarChaveAtivacao ($resource) {
    return $resource(address + '/api/gerarchaveativacao', { idRevenda: '@idRevenda' })
  }

  AtivacaoApp.factory('recursoClientes', recursoClientes)
  recursoClientes.$inject = ['$resource']
  function recursoClientes ($resource) {
    return $resource(address + '/api/clientes/:id', null, {
      update: {
        method: 'PUT'
      },
      query: {
        method: 'GET',
        params: { page: '@page', count: '@count', filter: '@filter' },
        isArray: true
      },
      get: {
        method: 'GET',
        params: { busca: '@busca' },
        isArray: false
      },
      save: {
        method: 'POST',
        transformResponse: function (data, headers) {
          var response = {}
          response.data = data
          response.headers = headers()
          return response
        }
      }
    })
  }

  AtivacaoApp.factory('recursoRevendas', recursoRevendas)
  recursoRevendas.$inject = ['$resource']
  function recursoRevendas ($resource) {
    return $resource(address + '/api/revendas/:id', null, {
      update: {
        method: 'PUT'
      }
    })
  }

  AtivacaoApp.factory('recursoTipoPdv', recursoTipoPdv)
  recursoTipoPdv.$inject = ['$resource']
  function recursoTipoPdv ($resource) {
    return $resource(address + '/api/tipopdvs/:id', null, {
      update: {
        method: 'PUT'
      }
    })
  }

  AtivacaoApp.factory('cadastroAtivacao', cadastroAtivacao)
  cadastroAtivacao.$inject = ['recursoAtivacao', '$q']
  function cadastroAtivacao (recursoAtivacao, $q) {
    return {
      salvar: function (ativacao) {
        return $q(function (resolve, reject) {
          if (ativacao.IDAtivacao) {
            recursoAtivacao.update({ id: ativacao.IDAtivacao }, ativacao, function (data) {
              var obj = { data: data, save: false }
              resolve(obj)
            }, function (err) {
              reject(err)
            })
          } else {
            recursoAtivacao.save(ativacao, function (data) {
              var obj = { data: JSON.parse(data.data), save: true }
              resolve(obj)
            }, function (err) {
              reject(err)
            })
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
          if (cliente.IDCliente) {
            recursoClientes.update({ id: cliente.IDCliente }, cliente, function () {
              resolve()
            }, function (err) {
              reject(err)
            })
          } else {
            recursoClientes.save(cliente, function (data) {
              resolve(JSON.parse(data.data).IDCliente)
            }, function (err) {
              reject(err)
            })
          }
        })
      }
    }
  }

  AtivacaoApp.factory('recursoAtivacaoOffline', recursoAtivacaoOffline)
  recursoAtivacaoOffline.$inject = ['$resource']
  function recursoAtivacaoOffline ($resource) {
    return $resource(address + '/api/validacaooffline/:id',
      null,
      {
        get: {
          method: 'GET',
          isArray: false
        }
      })
  }

  AtivacaoApp.factory('recursoBaixarLicenca', recursoBaixarLicenca)
  recursoBaixarLicenca.$inject = ['$resource']
  function recursoBaixarLicenca ($resource) {
    return $resource(address + '/api/baixarlicenca/:id',
      null,
      {
        get: {
          method: 'GET',
          isArray: false,
          transformResponse: function (data) {
            var response = {}
            response.data = data
            return response
          }
        }
      })
  }

  AtivacaoApp.factory('AuthenticationService', AuthenticationService)
  AuthenticationService.$inject = ['$http', '$cookieStore', '$rootScope', '$timeout', '$window', 'recursoUsuario']
  function AuthenticationService ($http, $cookieStore, $rootScope, $timeout, $window, recursoUsuario) {
    var service = {}

    service.Login = Login
    service.SetCredentials = SetCredentials
    service.ClearCredentials = ClearCredentials

    return service

    function Login (username, password, callback) {
      /* Dummy authentication for testing, uses $timeout to simulate api call
       ----------------------------------------------*/
      // $timeout(function () {
      //    var response
      //    UserService.GetByUsername(username)
      //        .then(function (user) {
      //            if (user !== null && user.password === password) {
      //                response = { success: true }
      //            } else {
      //                response = { success: false, message: 'Username or password is incorrect' }
      //            }
      //            callback(response)
      //        })
      // }, 1000)

      /* Use this for real authentication
       ----------------------------------------------*/
      $http.post(address + '/api/autenticacao', { email: username, senha: password })
        .then(function (response) {
          callback({ success: true, contents: response.data })
        }, function (response) {
          callback({ success: false, contents: response.data })
        })
    }

    function SetCredentials (username, password, jwt, adm) {
      var authdata = Base64.encode(username + ':' + password)
      $rootScope.globals = {
        currentUser: {
          username: username,
          authdata: authdata,
          jwt: jwt,
          adm: adm
        }
      }

      $http.defaults.headers.common['x-auth-token'] = jwt
      $cookieStore.put('globals', $rootScope.globals)
      $rootScope.$broadcast('logado', adm)
      $window.location.href = "#/ativacoes"
    }

    function ClearCredentials () {
      $rootScope.globals = {}
      $cookieStore.remove('globals')
      $http.defaults.headers.common['x-auth-token'] = null
      $rootScope.$broadcast('deslogado')
      $window.location.href = '#/login'
    }
  }

  // Base64 encoding service used by AuthenticationService
  const Base64 = {
    keyStr: 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=',

    encode: function (input) {
      var output = ''
      var chr1 = ''
      var chr2 = ''
      var chr3 = ''
      var enc1 = ''
      var enc2 = ''
      var enc3 = ''
      var enc4 = ''
      var i = 0

      do {
        chr1 = input.charCodeAt(i++)
        chr2 = input.charCodeAt(i++)
        chr3 = input.charCodeAt(i++)

        enc1 = chr1 >> 2
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4)
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6)
        enc4 = chr3 & 63

        if (isNaN(chr2)) {
          enc3 = enc4 = 64
        } else if (isNaN(chr3)) {
          enc4 = 64
        }

        output = output +
        this.keyStr.charAt(enc1) +
        this.keyStr.charAt(enc2) +
        this.keyStr.charAt(enc3) +
        this.keyStr.charAt(enc4)
        chr1 = chr2 = chr3 = ''
        enc1 = enc2 = enc3 = enc4 = ''
      } while (i < input.length)

      return output
    },

    decode: function (input) {
      var output = ''
      var chr1 = ''
      var chr2 = ''
      var chr3 = ''
      var enc1 = ''
      var enc2 = ''
      var enc3 = ''
      var enc4 = ''
      var i = 0

      // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
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

        output = output + String.fromCharCode(chr1)

        if (enc3 !== 64) {
          output = output + String.fromCharCode(chr2)
        }
        if (enc4 !== 64) {
          output = output + String.fromCharCode(chr3)
        }

        chr1 = chr2 = chr3 = ''
        enc1 = enc2 = enc3 = enc4 = ''
      } while (i < input.length)

      return output
    }
  }
})()
