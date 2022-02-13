   Integação CakeERP

---- PDVSEVEN ---- CAKE
Produtos       <> Sincroniza baseado nas datas (+1 dia, ou não existencia)
TipoPagamento  < (Desce agrupados)
Pedidos        => Sobe todos que tem produtos
Clietes        => Sobe todos de acordo com a existencia do pedido

A interface <IERP> permite o sincronismo de dados entre os sistemas

no pedido:
	order_type = 1,
    seller = 6316,
	Customer = (cliente em branco)

Atenção com os campos data, pois em alguns casos requer string, com formatação específica