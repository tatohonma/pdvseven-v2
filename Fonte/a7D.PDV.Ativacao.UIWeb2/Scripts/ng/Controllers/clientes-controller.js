;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .controller('ClientesController',
      ['recursoClientes', 'recursoRevendas', 'AuthenticationService', 'NgTableParams', function (recursoClientes, recursoRevendas, AuthenticationService, NgTableParams) {
        var self = this
        self.mensagem = ''
        self.clientes = []
        self.revendas = []
        self.tableParams = new NgTableParams({}, {
          getData: function (params) {
            return recursoClientes.query(params.url(), function (data, headersGetter) {
              var headers = headersGetter()
              var pages = parseInt(headers['count'], 10)
              params.total(pages)
              return data
            }).$promise
              .catch(function (err) {
                if (err.status === 401) {
                  AuthenticationService.ClearCredentials()
                }
                self.mensagem = err.data.Message
              })
          }
        })
        recursoRevendas.query(function (revendas) {
          angular.forEach(revendas, function (obj) {
            self.revendas.push({ id: obj.IDRevenda, title: obj.Nome })
          })
        }, function (err) {
          console.log(err)
        })
      }]
  )
})()
