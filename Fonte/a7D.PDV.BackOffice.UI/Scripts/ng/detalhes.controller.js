angular.module('backoffice')
  .controller('detalhes', detalhes)

detalhes.$inject = ["$window", "$scope"]


function detalhes($window, $scope) {
  var vm = this;
  vm.carregado = true;

  function erro(err) {
    vm.carregado = true;
    console.error(err);
  }

  function filtrar(diasInicio, diasFim) {
    vm.carregado = false;
    var data = moment().subtract(diasInicio, 'days').valueOf();

    var dataFim = null;
    if(diasFim != null)
      dataFim = moment().subtract(diasFim, 'days').valueOf();

    backoffice.obterRelatorioDetalhado(data, dataFim)
      .then(vm.atualizarDados)
      .catch(erro);
  }
  
  function filtrarEsteMes() {
    vm.carregado = false;
    var base = moment();

    var inicio = base.startOf('month').valueOf();
    var fim = base.endOf('month').valueOf();

    backoffice.obterRelatorioDetalhado(inicio, fim)
      .then(vm.atualizarDados)
      .catch(erro);
  }

  function filtrarMesPassado() {
    vm.carregado = false;
    var base = moment().subtract(1, 'month');

    var inicio = base.startOf('month').valueOf();
    var fim = base.endOf('month').valueOf();

    backoffice.obterRelatorioDetalhado(inicio, fim)
      .then(vm.atualizarDados)
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
  ];

  vm.atualizarDados = function(strData) {
    var data = angular.fromJson(strData);

    faturamentoCategoria.config.data = data.categoria;
    faturamentoTipoPagamento.config.data = data.tipoPagamento;
    faturamentoTipoPedido.config.data = data.tipoPedido;
    faturamentoDiaSemana.config.data = data.diaDaSemana;
    motivosCancelamento.config.data = data.motivosCancelamento;
    volumeGarcom.config.data = data.volumeGarcom;

    faturamentoCategoria.update();
    faturamentoTipoPagamento.update();
    faturamentoTipoPedido.update();
    faturamentoDiaSemana.update();
    motivosCancelamento.update();
    volumeGarcom.update();
    vm.carregado = true;
    $scope.$apply();
  }
}