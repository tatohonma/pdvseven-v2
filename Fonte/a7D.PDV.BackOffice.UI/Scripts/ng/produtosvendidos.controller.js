angular.module('backoffice')
  .controller('produtosVendidos', produtosVendidos);

produtosVendidos.$inject = ["$window", "$scope", "NgTableParams"]

function produtosVendidos($window, $scope, NgTableParams) {
  var vm  = this;

  function erro(err) {
    vm.carregadoQuantidade = true;
    vm.carregadoValor = true;
    vm.carregadoTotal = true;
  }

  function filtrar(dias, diasFim) {
    vm.carregadoQuantidade = false;
    vm.carregadoValor = false;
    vm.carregadoTotal = false;

    var data = moment().subtract(dias, 'days').valueOf();

    var dataFim = null;
    if(diasFim != null)
      dataFim = moment().subtract(diasFim, 'days').valueOf();

    backoffice.produtosVendidos('Quantidade', data, dataFim)
      .then(vm.atualizarDadosQuantidade)
      .catch(erro);

    backoffice.produtosVendidos('Valor', data, dataFim)
      .then(vm.atualizarDadosValor)
      .catch(erro);
  }

  function filtrarEsteMes() {
    vm.carregadoQuantidade = false;
    vm.carregadoValor = false;
    vm.carregadoTotal = false;

    var base = moment();

    var inicio = base.startOf('month').valueOf();
    var fim = base.endOf('month').valueOf();

    backoffice.produtosVendidos('Quantidade', inicio, fim)
      .then(vm.atualizarDadosQuantidade)
      .catch(erro);

    backoffice.produtosVendidos('Valor', inicio, fim)
      .then(vm.atualizarDadosValor)
      .catch(erro);
  }

  function filtrarMesPassado() {
    vm.carregadoQuantidade = false;
    vm.carregadoValor = false;
    vm.carregadoTotal = false;
    
    var base = moment().subtract(1, 'month');

    var inicio = base.startOf('month').valueOf();
    var fim = base.endOf('month').valueOf();

    backoffice.produtosVendidos('Quantidade', inicio, fim)
      .then(vm.atualizarDadosQuantidade)
      .catch(erro);

    backoffice.produtosVendidos('Valor', inicio, fim)
      .then(vm.atualizarDadosValor)
      .catch(erro);
  }

  vm.acoes = [
 { id: 1, nome: "Hoje", filtro: filtrar.bind(this, 0, 0)},
  { id: 2, nome: "Ontem", filtro: filtrar.bind(this, 1, 1)},
  { id: 3, nome: "Este mês", filtro: filtrarEsteMes.bind(this)},
  { id: 4, nome: "Mês passado", filtro: filtrarMesPassado.bind(this)},
  { id: 5, nome: "Últimos 7 dias", filtro: filtrar.bind(this, 7, null)},
  { id: 6, nome: "Últimos 30 dias", filtro: filtrar.bind(this, 30, null)},
  { id: 7, nome: "Últimos 60 dias", filtro: filtrar.bind(this, 60, null)},
  { id: 8, nome: "Todo o período", filtro: filtrar.bind(this, 3600, null)},
  ]

  vm.maisVendidos = new NgTableParams({ count: 10 }, { counts: [], dataset: [] });
  vm.menosVendidos = new NgTableParams({ count: 10 }, { counts: [], dataset: [] });

  vm.maisVendidosValor = new NgTableParams({ count: 10 }, { counts: [], dataset: [] });
  vm.menosVendidosValor = new NgTableParams({ count: 10 }, { counts: [], dataset: [] });


  vm.carregadoQuantidade = false;
  vm.carregadoValor = false;
  vm.carregadoTotal = false;

  vm.atualizarDadosQuantidade = function(strData) {
    var data = angular.fromJson(strData);
    vm.maisVendidos.settings({
      dataset: data.maisVendidos
    });
    vm.menosVendidos.settings({
      dataset: data.menosVendidos
    });
    vm.carregadoQuantidade = true;
    vm.carregadoTotal = vm.carregadoQuantidade && vm.carregadoValor;
    $scope.$apply();
  }

  vm.atualizarDadosValor = function(strData) {
    var data = angular.fromJson(strData);
    vm.maisVendidosValor.settings({
      dataset: data.maisVendidos
    });
    vm.menosVendidosValor.settings({
      dataset: data.menosVendidos
    });
    vm.carregadoValor = true;
    vm.carregadoTotal = vm.carregadoQuantidade && vm.carregadoValor;
    $scope.$apply();
  }
}