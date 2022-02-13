angular.module('AtivacaoApp')
  .controller('AtivacaoController', AtivacaoController)

AtivacaoController.$inject = ['$scope', '$rootScope', '$filter', '$location', '$routeParams', '$timeout', 'recursoClientes', 'recursoAtivacao', 'recursoGerarChaveAtivacao', 'cadastroAtivacao', 'AuthenticationService', 'recursoTipoPdv', '_']
function AtivacaoController ($scope, $rootScope, $filter, $location, $routeParams, $timeout, recursoClientes, recursoAtivacao, recursoGerarChaveAtivacao, cadastroAtivacao, AuthenticationService, recursoTipoPdv, _) {
  var self = this
  self.carregado = false
  self.ativacao = {}
  self.mensagem = null
  self.mensagemErr = null
  self.revenda = 0
  self.quantidade = 1
  self.tiposPdv = []
  self.cliente = {}
  self.bMensagem = null


  var adm = $rootScope.globals.currentUser.adm === true

  if(!adm) {
    $location.path('/ativacoes')
    return
  }

  recursoTipoPdv.query(function (dados) {
      self.tiposPdv = dados
      console.log(self.tiposPdv);
  })

  if ($routeParams.id) {
    recursoAtivacao.get({ id: $routeParams.id }, function (ativacao) {
      self.carregado = true
        self.ativacao = ativacao
        self.ativacao.SiteAdmin = true
    }, function (err) {
      var msg = err.data.Message || ''
      self.carregado = true
      if (err.status === 401) {
        AuthenticationService.ClearCredentials()
      }
      if (err.status === 404) {
        msg = 'Registro não encontrado'
        self.carregado = false
      }
      self.mensagemErr = msg
    })
  } else {
    self.ativacao.IDAtivacao = 0
    self.carregado = true
    self.ativacao.Ativo = true
  }

  self.salvar = function (ativacao) {
    self.enviando = true
    self.ativacao.IDCliente = self.ativacao.Cliente.IDCliente
    cadastroAtivacao.salvar(self.ativacao).then(function (ret) {
      self.mensagem = 'Salvo com sucesso!'
      if (ret.save === true) {
        self.carregado = false
        $location.path('/ativacoes/edit/' + ret.data.IDAtivacao)
      }
      self.ativacao = ret.data
      self.enviando = false
    })
      .catch(function (err) {
        if (err.status === 401) {
          AuthenticationService.ClearCredentials()
        }
        self.mensagemErr = err.data.Message
        self.enviando = false
      })
  }

  self.gerarChave = function () {
    recursoGerarChaveAtivacao.get({ idRevenda: self.ativacao.Revenda }, function (chave) {
      self.ativacao.ChaveAtivacao = chave.ChaveAtivacao
    }, function (err) {
      if (err.status === 401) {
        AuthenticationService.ClearCredentials()
      }
      self.mensagemErr = err.data.Message
    })
  }

  self.adicionarLicenca = function (idTipoPdv) {
    self.ativacao.PDVs = self.ativacao.PDVs || []
    for (var i = 0; i < self.quantidade; i++) {
      var licenca = {}
      licenca.IDAtivacao = self.ativacao.IDAtivacao
      licenca.IDTipoPDV = idTipoPdv
      licenca.Nome = self.nomePDV(idTipoPdv) + " " + (self.ativacao.PDVs.length+1)
      licenca.Deletado = false
      licenca.Ativo = true
      self.ativacao.PDVs.unshift(licenca)
    }
    self.quantidade = 1
  }

  self.alterarStatus = function (licenca) {
      if (licenca.IDAtivacao) {
        licenca.Ativo = !licenca.Ativo;
    }
  }

  self.fecharErro = function () {
    self.mensagemErr = null
  }

  self.fecharMensagem = function () {
    self.mensagem = null
  }

  self.getClientes = function (val) {
    return recursoClientes.query({ 'filter[Nome]': val })
      .$promise
      .then(function (clientes) {
        return clientes
      })
  }
    
  self.nomePDV = function (idTipoPdv) {
    var i;
    for (i = 0; i < self.tiposPdv.length; i++) {
      if (self.tiposPdv[i].IDTipoPDV == idTipoPdv)
        return self.tiposPdv[i].Nome;
    }
    return "desconhecido(" + idTipoPdv + ")";
  }
}