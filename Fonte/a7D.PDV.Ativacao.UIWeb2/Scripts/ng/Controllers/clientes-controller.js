;(function () {
    'use strict'
    angular
        .module('AtivacaoApp')
        .controller('ClientesController',
            ['recursoClientes', 'recursoRevendas', 'AuthenticationService', 'NgTableParams' , 'AccountContext', function (recursoClientes, recursoRevendas, AuthenticationService, NgTableParams, AccountContext) {
                var self = this
                self.mensagem = ''
                self.clientes = []
                self.revendas = []


                self.hasRole    = function (role)     { return AccountContext.hasRole(role); };
                self.hasAnyRole = function (required) { return AccountContext.hasAnyRole(required || []); };
                self.hasPerm    = function (perm)     { return AccountContext.hasPermission(perm); };

                self.isAdmin = AccountContext.hasRole('admin');
                
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
                        self.revendas.push({id: obj.IDRevenda, title: obj.Nome})
                    })
                }, function (err) {
                    console.log(err)
                })
            }]
        )
})()
