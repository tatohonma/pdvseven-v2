;(function () {
    'use strict'

    angular
        .module('AtivacaoApp')
        .controller('ClienteController', ClienteController)

    ClienteController.$inject = [
        '$scope', '$location', '$routeParams', '$timeout',
        'recursoClientes', 'cadastroCliente', 'AuthenticationService'
    ]
    function ClienteController (
        $scope, $location, $routeParams, $timeout,
        recursoClientes, cadastroCliente, AuthenticationService
    ) {
        $scope.carregado = false
        $scope.cliente = {}
        $scope.mensagem = null
        $scope.mensagemErr = null
        $scope.doc = 'CNPJ'

        function fromApi(c) {
            c = c || {}
            return {
                id: c.id || 0,
                resellerId: c.resellerId || null,
                reseller: c.reseller || null,
                name: c.name || '',
                companyName: c.companyName || '',
                cpfCnpj: c.cpfCnpj || '',
                street: c.street || '',
                number: c.number || '',
                additionalInfo: c.additionalInfo || '',
                city: c.city || '',
                state: c.state || '',
                phone: c.phone || '',
                tinyId: c.tinyId || null,
                createdAt: c.createdAt || null,
                updatedAt: c.updatedAt || null
            }
        }

        function toApi(v) {
            v = v || {}
            return {
                id: v.id || 0,
                resellerId: v.resellerId || (v.reseller && v.reseller.id) || null,
                name: v.name,
                companyName: v.companyName,
                cpfCnpj: v.cpfCnpj,
                street: v.street,
                number: v.number,
                additionalInfo: v.additionalInfo,
                city: v.city,
                state: v.state,
                phone: v.phone,
                tinyId: v.tinyId
            }
        }

        if ($routeParams.id) {
            recursoClientes.get({ id: $routeParams.id }, function (cliente) {
                $scope.carregado = true
                $scope.cliente = fromApi(cliente)
                var len = ($scope.cliente.cpfCnpj || '').toString().length
                $scope.doc = (len === 14 ? 'CNPJ' : 'CPF')
            }, function (err) {
                if (err.status === 401) AuthenticationService.ClearCredentials()
                $scope.carregado = true
                $scope.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Erro ao carregar'
            })
        } else {
            $scope.cliente = fromApi({})
            $scope.carregado = true
        }

        $scope.salvar = function (clienteView) {
            var payload = toApi(clienteView)
            cadastroCliente.salvar(payload).then(function (id) {
                $scope.mensagem = 'Salvo com sucesso!'
                if (id) {
                    $scope.carregado = false
                    $timeout(function () { $location.path('/clientes/edit/' + id) }, 1500)
                }
            }).catch(function (err) {
                if (err.status === 401) AuthenticationService.ClearCredentials()
                $scope.mensagemErr = (err.data && (err.data.Message || err.data.message)) || 'Erro ao salvar'
            })
        }
    }
})()
