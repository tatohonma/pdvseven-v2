﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace a7D.PDV.EF.Properties {
    using System;
    
    
    /// <summary>
    ///   Uma classe de recurso de tipo de alta segurança, para pesquisar cadeias de caracteres localizadas etc.
    /// </summary>
    // Essa classe foi gerada automaticamente pela classe StronglyTypedResourceBuilder
    // através de uma ferramenta como ResGen ou Visual Studio.
    // Para adicionar ou remover um associado, edite o arquivo .ResX e execute ResGen novamente
    // com a opção /str, ou recrie o projeto do VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retorna a instância de ResourceManager armazenada em cache usada por essa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("a7D.PDV.EF.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Substitui a propriedade CurrentUICulture do thread atual para todas as
        ///   pesquisas de recursos que usam essa classe de recurso de tipo de alta segurança.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a BEGIN TRANSACTION T1
        ///
        ///SET IDENTITY_INSERT [dbo].[tbEstado] ON 
        ///INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (1, N&apos;Acre&apos;, N&apos;AC&apos;)
        ///INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (2, N&apos;Alagoas&apos;, N&apos;AL&apos;)
        ///INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (3, N&apos;Amapá&apos;, N&apos;AP&apos;)
        ///INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (4, N&apos;Amazonas&apos;, N&apos;AM&apos;)
        ///INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (5, N&apos;Bahia&apos;, N&apos;BA&apos;)
        ///INSERT [dbo].[tbEstado] ([I [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Outros {
            get {
                return ResourceManager.GetString("Outros", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT c.NomeCompleto, s.dtMovimento [Data Pedido], s.IDPedido [N.Pedido], S.Valor Credito,
        ///	(SELECT sum(valor) FROM tbsaldo d WHERE d.tipo=&apos;D&apos; AND d.IDPai=s.idSaldo) Debito,
        ///	S.Valor - (SELECT sum(valor) FROM tbsaldo d WHERE d.tipo=&apos;D&apos; AND d.IDPai=s.idSaldo) Saldo
        ///FROM tbsaldo s
        ///INNER JOIN tbCliente c ON c.IDCliente=s.IDCliente
        ///WHERE s.tipo=&apos;C&apos;
        ///AND s.dtMovimento BETWEEN @dtInicio AND @dtFim
        ///ORDER BY c.NomeCompleto;.
        /// </summary>
        internal static string Relatorio_Creditos {
            get {
                return ResourceManager.GetString("Relatorio_Creditos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT 
        ///   p.IDPedido as &apos;Cód. Pedido&apos;
        ///  ,u.Nome as &apos;Usuário fechamento&apos;
        ///  ,cl.NomeCompleto as &apos;Cliente&apos;
        ///  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as &apos;Valor produto (R$)&apos;
        ///  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as &apos;Valor entrada (R$)&apos;
        ///  ,ISNULL(p.ValorServico, 0) as &apos;Valor serviço (R$)&apos;
        ///  ,ISNULL(p.ValorDesconto, 0) as &apos;Valor descont [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Desconto_por_cliente {
            get {
                return ResourceManager.GetString("Relatorio_Desconto_por_cliente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///   NomeCompleto
        ///  ,(&apos;(&apos; + CAST(ISNULL(Telefone1DDD, 11) as VARCHAR(2)) + &apos;) &apos; + CAST(Telefone1Numero as VARCHAR(9))) as &apos;Telefone&apos;
        ///  ,(CAST(DAY(DataNascimento) as VARCHAR(2)) + &apos;/&apos; + CAST(MONTH(DataNascimento) as VARCHAR(2))) as &apos;Aniversário&apos;
        ///  ,case c.Sexo
        ///    when &apos;m&apos; then &apos;Homem&apos;
        ///    when &apos;f&apos; then &apos;Mulher&apos; END &apos;Tipo&apos;
        ///  ,Documento1 as &apos;CPF/CNPJ&apos;
        ///  ,Endereco + &apos;, &apos; + EnderecoNumero as &apos;Endereço&apos;
        ///  ,Bairro
        ///  ,Cidade
        ///  ,e.Nome as UF
        ///  ,c.CEP
        ///  ,DtInclusao as &apos;Data cadastro&apos;
        ///FROM
        ///  tbCli [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Lista_de_Clientes {
            get {
                return ResourceManager.GetString("Relatorio_Lista_de_Clientes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT p.IDPedido AS &apos;Cód. Pedido&apos; ,
        ///       u.Nome AS &apos;Usuário fechamento&apos; ,
        ///       cl.NomeCompleto AS &apos;Cliente&apos; ,
        ///       ISNULL(
        ///                (SELECT SUM(ValorUnitario*Quantidade)
        ///                 FROM tbPedidoProduto
        ///                 WHERE idPedido=p.idPedido
        ///                   AND idProduto NOT IN (2, 3)), 0) AS &apos;Valor produto (R$)&apos; ,
        ///       ISNULL(
        ///                (SELECT SUM(ValorUnitario*Quantidade)
        ///                 FROM tbPedidoProduto
        ///                 WHERE idPedido=p.idPedido
        ///        [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Lista_de_Pedidos {
            get {
                return ResourceManager.GetString("Relatorio_Lista_de_Pedidos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a select 
        ///	 a.Nome as &apos;Nome&apos;
        ///	,b.Nome as &apos;Classificação Fiscal&apos;
        ///	,a.ValorUnitario as &apos;Valor&apos;
        ///	,b.NCM as &apos;NCM&apos;
        ///	,c.CFOP as &apos;CFOP&apos;
        ///	,b.IOF as &apos;IOF&apos;
        ///	,b.IPI as &apos;IPI&apos;
        ///	,b.PISPASEP as &apos;PISPASEP&apos;
        ///	,b.CIDE as &apos;CIDE&apos;
        ///	,b.COFINS as &apos;COFINS&apos;
        ///	,b.ICMS as &apos;ICMS&apos;
        ///	,b.ISS as &apos;ISS&apos;
        ///
        ///FROM
        ///	tbproduto a
        ///	INNER JOIN tbClassificacaoFiscal b ON a.IDClassificacaoFiscal=b.IDClassificacaoFiscal
        ///	INNER JOIN tbTipoTributacao c ON c.IDTipoTributacao=b.IDTipoTributacao
        ///WHERE
        ///	 Excluido=0
        ///	 AND
        ///	 Ativo=1
        ///	 AND
        ///	 Id [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Lista_Produtos_Impostos {
            get {
                return ResourceManager.GetString("Relatorio_Lista_Produtos_Impostos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT 
        ///	 p.IDPedido as &apos;Cód. Pedido&apos;
        ///	,u.Nome as &apos;Usuário fechamento&apos;
        ///	,cl.NomeCompleto as &apos;Cliente&apos;
        ///	,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as &apos;Valor produto (R$)&apos;
        ///	,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as &apos;Valor entrada (R$)&apos;
        ///	,ISNULL(p.ValorServico, 0) as &apos;Valor serviço (R$)&apos;
        ///	,ISNULL(p.ValorDesconto, 0) as &apos;Valor desconto (R$)&apos; [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Motivo_Desconto {
            get {
                return ResourceManager.GetString("Relatorio_Motivo_Desconto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT 
        ///   p.IDPedido as &apos;Cód. Pedido&apos;
        ///  ,u.Nome as &apos;Usuário fechamento&apos;
        ///  ,cl.NomeCompleto as &apos;Cliente&apos;
        ///  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as &apos;Valor produto (R$)&apos;
        ///  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as &apos;Valor entrada (R$)&apos;
        ///  ,ISNULL(p.ValorServico, 0) as &apos;Valor serviço (R$)&apos;
        ///  ,ISNULL(p.ValorDesconto, 0) as &apos;Valor descont [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Motivos_de_Desconto {
            get {
                return ResourceManager.GetString("Relatorio_Motivos_de_Desconto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT p.IDPedido AS &apos;Cód. Pedido&apos; ,
        ///       u.Nome AS &apos;Usuário fechamento&apos; ,
        ///       cl.NomeCompleto AS &apos;Cliente&apos; ,
        ///       ISNULL(
        ///                (SELECT SUM(ValorUnitario*Quantidade)
        ///                 FROM tbPedidoProduto
        ///                 WHERE idPedido=p.idPedido
        ///                   AND idProduto NOT IN (2, 3)), 0) AS &apos;Valor produto (R$)&apos; ,
        ///       ISNULL(
        ///                (SELECT SUM(ValorUnitario*Quantidade)
        ///                 FROM tbPedidoProduto
        ///                 WHERE idPedido=p.idPedido
        ///        [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Pedidos_com_Desconto {
            get {
                return ResourceManager.GetString("Relatorio_Pedidos_com_Desconto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///   p.idPedido as &apos;Cód. Pedido&apos;
        ///  ,cl.NomeCompleto as &apos;Cliente&apos;
        ///  ,p1.Nome as &apos;PDV lançamento&apos;
        ///  ,u1.Nome as &apos;Usuário lançamento&apos;
        ///  ,pp.DtInclusao as &apos;Data lançamento&apos;
        ///  ,p2.Nome as &apos;PDV cancelamento&apos;
        ///  ,u2.Nome as &apos;Usuário cancelamento&apos;
        ///  ,pp.DtCancelamento &apos;Data cancelamento&apos;
        ///  ,pr.Nome as &apos;Produto&apos;
        ///  ,pp.Quantidade as &apos;Qtd.&apos;
        ///  ,pp.ValorUnitario as &apos;Valor unitário (R$)&apos;
        ///    ,(pp.ValorUnitario*pp.Quantidade) as &apos;Valor total (R$)&apos;
        ///  ,mc.Nome as &apos;Motivo Cancelamento&apos;
        ///FROM
        ///  tbPedidoProdu [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Cancelados {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Cancelados", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///     p1.Nome as &apos;PDV Cancelamento&apos;
        ///  ,u1.Nome as &apos;Usuário Cancelamento&apos;
        ///  ,SUM(pp.Quantidade) as &apos;Qtd.&apos;
        ///  ,SUM(pp.ValorUnitario*pp.Quantidade) as &apos;Valor total (R$)&apos;
        ///FROM
        ///  tbPedidoProduto pp
        ///  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
        ///  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
        ///  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario_cancelamento
        ///  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV_cancelamento
        ///  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
        ///  LEFT JOIN tbCaixa c ON [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Cancelados_Usuario_Cancelamento {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Cancelados_Usuario_Cancelamento", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///     p1.Nome as &apos;PDV lançamento&apos;
        ///  ,u1.Nome as &apos;Usuário lançamento&apos;
        ///  ,SUM(pp.Quantidade) as &apos;Qtd.&apos;
        ///  ,SUM(pp.ValorUnitario*pp.Quantidade) as &apos;Valor total (R$)&apos;
        ///FROM
        ///  tbPedidoProduto pp
        ///  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
        ///  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
        ///  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
        ///  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
        ///  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
        ///  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
        ///WHERE
        ///  [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Cancelados_Usuario_Lancamento {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Cancelados_Usuario_Lancamento", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT p.idPedido AS &apos;Cód. Pedido&apos; ,
        ///       cl.NomeCompleto AS &apos;Cliente&apos; ,
        ///       p1.Nome AS &apos;PDV lançamento&apos; ,
        ///       u1.Nome AS &apos;Usuário lançamento&apos; ,
        ///       pp.DtInclusao AS &apos;Data lançamento&apos; ,
        ///  (SELECT TOP 1 c.Nome
        ///   FROM tbProdutoCategoriaProduto pcp
        ///   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
        ///   AND pcp.IDProduto = pp.IDProduto) AS &apos;Categoria&apos; ,
        ///       pr.Nome AS &apos;Produto&apos; ,
        ///       pp.Quantidade AS &apos;Qtd.&apos; ,
        ///       pp.ValorUnitario AS &apos;Valor unitário [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Vendidos {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Vendidos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///  (SELECT TOP 1 c.Nome
        ///   FROM tbProdutoCategoriaProduto pcp
        ///   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
        ///   AND pcp.IDProduto = p.IDProduto) AS &apos;Categoria&apos; ,
        ///       p.Nome AS &apos;Item&apos; ,
        ///       pp.Quantidade AS &apos;Qtd.&apos; ,
        ///       pp.ValorUnitario AS &apos;Valor unit.&apos; ,
        ///       CAST((pp.ValorUnitario*pp.Quantidade) AS DECIMAL(18, 2)) &apos;Valor total&apos; ,
        ///                                                                pp.DtInclusao AS &apos;Data&apos; ,
        ///                          [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Vendidos_2 {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Vendidos_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a select
        ///    isnull(tbCliente.NomeCompleto, &apos;Sem Cliente&apos;) as &apos;Cliente&apos;
        ///  , tbProduto.Codigo as &apos;Codigo Produto&apos;
        ///  , tbProduto.Nome as &apos;Produto&apos;
        ///  , sum(tbPedidoProduto.Quantidade) as Quantidade
        ///  , tbPedidoProduto.ValorUnitario
        ///  , cast(sum(tbPedidoproduto.Quantidade * tbPedidoProduto.ValorUnitario) as decimal(10, 2)) as &apos;Valor&apos;
        ///  , cast(tbPedido.DtPedidoFechamento as date) as &apos;Data Fechamento Pedido&apos;
        ///from tbPedidoProduto (nolock)
        ///inner join tbProduto (nolock) on tbProduto.IDProduto = tbPedidoProdut [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Vendidos_por_Cliente {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Vendidos_por_Cliente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///  (SELECT TOP 1 c.Nome
        ///   FROM tbProdutoCategoriaProduto pcp
        ///   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
        ///   AND pcp.IDProduto = pp.IDProduto) AS &apos;Categoria&apos; ,
        ///       pr.Nome AS &apos;Produto&apos; ,
        ///       SUM(pp.Quantidade) AS &apos;Qtd.&apos; ,
        ///       SUM(pp.ValorUnitario*pp.Quantidade) AS &apos;Valor total (R$)&apos;
        ///FROM tbPedidoProduto pp
        ///LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
        ///LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
        ///LEFT JOIN tbUsuario u1 ON u1.idUsuario=p [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Vendidos_Resumo {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Vendidos_Resumo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///  (SELECT TOP 1 c.Nome
        ///   FROM tbProdutoCategoriaProduto pcp
        ///   INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = pcp.IDCategoriaProduto
        ///   AND pcp.IDProduto = p.IDProduto) AS &apos;Categoria&apos; ,
        ///       p.Nome AS &apos;Item&apos; ,
        ///       SUM(pp.Quantidade) AS &apos;Qtd.&apos; ,
        ///       CAST(SUM(pp.ValorUnitario*pp.Quantidade) AS DECIMAL(18, 2)) &apos;Valor total&apos;
        ///FROM tbPedidoProduto pp
        ///INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
        ///INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
        ///INNER JOIn tbPedido pd ON [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Produtos_Vendidos_Resumo_2 {
            get {
                return ResourceManager.GetString("Relatorio_Produtos_Vendidos_Resumo_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT CASE cl.Sexo
        ///           WHEN &apos;m&apos; THEN &apos;Homem&apos;
        ///           WHEN &apos;f&apos; THEN &apos;Mulher&apos;
        ///       END &apos;Tipo&apos; ,COUNT(DISTINCT(p.idPedido)) AS &apos;Qtd.&apos; ,
        ///  (SELECT SUM(ValorUnitario*Quantidade)
        ///   FROM tbPedidoProduto pp1
        ///   INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
        ///   INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
        ///   INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
        ///   WHERE pp1.Cancelado=0
        ///     AND c1.idFechamento=@idFechamento
        ///     AND cl1.Sexo=cl.Sexo
        ///     AND p1.idstatuspedido = 40
        ///  [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Resumo_por_Sexo {
            get {
                return ResourceManager.GetString("Relatorio_Resumo_por_Sexo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///   te.Nome as &apos;Tipo&apos;
        ///  ,COUNT(DISTINCT(p.idPedido)) as &apos;Qtd.&apos;
        ///FROM
        ///  tbPedido p (NOLOCK)
        ///  LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
        ///  LEFT JOIN tbTipoEntrada te (NOLOCK) ON te.idTipoEntrada=p.idTipoEntrada
        ///WHERE
        ///  c.idFechamento=@idFechamento
        ///  AND
        ///  te.idTipoEntrada IS NOT NULL
        ///GROUP BY
        ///   te.idTipoEntrada
        ///  ,te.Nome.
        /// </summary>
        internal static string Relatorio_Resumo_por_Tipo_de_Entrada {
            get {
                return ResourceManager.GetString("Relatorio_Resumo_por_Tipo_de_Entrada", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT tp.Nome &apos;Tipo pagamento&apos; ,
        ///               COUNT(DISTINCT(pp.IDTipoPagamento)) AS &apos;Qtd.&apos; ,
        ///               SUM(pp.Valor) &apos;Valor total (R$)&apos;
        ///FROM tbCaixa c
        ///LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
        ///LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
        ///LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
        ///WHERE idFechamento=@idFechamento
        ///  AND pp.idTipoPagamento IS NOT NULL
        ///  AND p.idStatusPedido = 40
        ///  AND pp.Excluido=0
        ///GROUP BY tp.Nome.
        /// </summary>
        internal static string Relatorio_Resumo_por_Tipo_Pagamento {
            get {
                return ResourceManager.GetString("Relatorio_Resumo_por_Tipo_Pagamento", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a select
        ///	tbProduto.IDProduto,
        ///	tbProduto.Nome,
        ///	sum(
        ///		(case when Entrada = 1
        ///		 then tbEntradaSaida.Quantidade
        ///		 else tbEntradaSaida.Quantidade * -1
        ///		 end)
        ///		 ) Quantidade
        ///from tbEntradaSaida (nolock)
        ///inner join tbProduto (nolock) on tbProduto.IDProduto = tbEntradaSaida.IDProduto
        ///where not exists (select 1 from tbPedido (nolock) where GUIDMovimentacao = tbEntradaSaida.GUID_Origem)
        ///and not exists (select 1 from tbMovimentacao (nolock) where GUID = tbEntradaSaida.GUID_Origem)
        ///and not exists (se [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Saidas_Avulsas {
            get {
                return ResourceManager.GetString("Relatorio_Saidas_Avulsas", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///	 Descricao as &apos;Descrição da Sangria&apos; 
        ///	,Valor as &apos;Valor da Sangria&apos;
        ///	,DtAjuste as &apos;Data.&apos;
        ///FROM 
        ///	tbCaixaAjuste
        ///WHERE
        ///	IDCaixaTipoAjuste=30
        ///	AND
        ///	DtAjuste BETWEEN  @dtInicio AND  @dtFim
        ///.
        /// </summary>
        internal static string Relatorio_Sangria {
            get {
                return ResourceManager.GetString("Relatorio_Sangria", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///
        ///  c.Nome as &apos;Nome Entregador&apos;
        ///  ,pp.Valor as &apos;Valor da Taxa&apos;
        ///  ,COUNT (p.ValorEntrega) as &apos;Quantidade&apos;
        ///  ,SUM (p.ValorEntrega) as &apos;Valor Total&apos;
        ///  
        ///FROM
        ///  tbpedido p
        ///  INNER JOIN tbTaxaEntrega pp ON p.IDTaxaEntrega=pp.IDTaxaEntrega
        ///  INNER JOIN tbEntregador c ON p.IDEntregador=c.IDEntregador
        ///
        ///WHERE
        ///  p.IDStatusPedido=40
        ///  AND
        ///  p.IDTipoPedido=30
        ///  AND
        ///  p.DtPedido BETWEEN @dtInicio AND @dtFim
        ///GROUP BY
        ///  c.Nome
        ///  ,pp.Valor
        ///  ,p.ValorEntrega
        ///  ,ValorEntrega.
        /// </summary>
        internal static string Relatorio_Taxa_de_Entrega_Delivery {
            get {
                return ResourceManager.GetString("Relatorio_Taxa_de_Entrega_Delivery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///   u.Nome
        ///  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade) as money) as &apos;Vendido&apos;
        ///  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*0.1) as money) as &apos;Serviço&apos;  
        ///  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*(1-pp.valorDesconto/ValorTotal)) as money) as &apos;Vendido com Desconto&apos;
        ///  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade*(1-pp.valorDesconto/ValorTotal)*0.1) as money) as &apos;Serviço com Desconto&apos;
        ///FROM 
        ///  tbPedido p
        ///  LEFT JOIN tbPedidoProduto pp ON pp.IDPedido=p.IDPedido AND pp.Cancelado=0 AND pp.IDProduto&lt;&gt;4
        ///  L [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Taxa_de_servico_por_garcom {
            get {
                return ResourceManager.GetString("Relatorio_Taxa_de_servico_por_garcom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT
        ///
        ///	c.Nome as &apos;Nome Entregador&apos;
        ///	,pp.Valor as &apos;Valor da Taxa&apos;
        ///	,COUNT (p.ValorEntrega) as &apos;Quantidade&apos;
        ///	,SUM (p.ValorEntrega) as &apos;Valor Total&apos;
        ///	
        ///FROM
        ///	tbpedido p
        ///	INNER JOIN tbTaxaEntrega pp ON p.IDTaxaEntrega=pp.IDTaxaEntrega
        ///	INNER JOIN tbEntregador c ON p.IDEntregador=c.IDEntregador
        ///
        ///WHERE
        ///	p.IDStatusPedido=40
        ///	AND
        ///	p.IDTipoPedido=30
        ///	AND
        ///	p.DtPedido BETWEEN @dtInicio AND @dtFim
        ///GROUP BY
        ///	c.Nome
        ///	,pp.Valor
        ///	,p.ValorEntrega
        ///	,ValorEntrega.
        /// </summary>
        internal static string Relatorio_Taxa_Entrega_Delivery {
            get {
                return ResourceManager.GetString("Relatorio_Taxa_Entrega_Delivery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a select 
        ///	tp.Nome as &apos;Forma Pagamento&apos;, 
        ///	m.Descricao as &apos;Metodo&apos;, 
        ///	b.Nome &apos;Bandeira&apos;, 
        ///	sum(pp.Valor) &apos;Valor Total (R$)&apos;
        ///FROM tbPedido p
        ///INNER JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
        ///INNER JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
        ///INNER JOIN tbBandeira b ON pp.IDBandeira=b.IDBandeira
        ///INNER JOIN tbMeioPagamentoSAT m ON m.IDMeioPagamentoSAT=pp.IDMetodo
        ///WHERE pp.Excluido=0
        ///AND p.DtPedidoFechamento BETWEEN @dtInicio AND @dtFim
        ///AND p.idStatusPedido=40
        ///GROUP BY tp. [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Relatorio_Tipos_Metodo_Bandeira {
            get {
                return ResourceManager.GetString("Relatorio_Tipos_Metodo_Bandeira", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a BEGIN TRANSACTION T1
        ///
        ///INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (10, N&apos;Não Iniciado&apos;)
        ///INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (20, N&apos;Processando&apos;)
        ///INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (30, N&apos;Sucesso&apos;)
        ///INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (40, N&apos;Abortado&apos;)
        ///INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [ [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string Tipos {
            get {
                return ResourceManager.GetString("Tipos", resourceCulture);
            }
        }
    }
}
