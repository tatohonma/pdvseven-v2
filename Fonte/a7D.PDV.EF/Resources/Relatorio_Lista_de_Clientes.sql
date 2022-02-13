SELECT
   NomeCompleto
  ,('(' + CAST(ISNULL(Telefone1DDD, 11) as VARCHAR(2)) + ') ' + CAST(Telefone1Numero as VARCHAR(9))) as 'Telefone'
  ,(CAST(DAY(DataNascimento) as VARCHAR(2)) + '/' + CAST(MONTH(DataNascimento) as VARCHAR(2))) as 'Aniversário'
  ,case c.Sexo
    when 'm' then 'Homem'
    when 'f' then 'Mulher' END 'Tipo'
  ,Documento1 as 'CPF/CNPJ'
  ,Endereco + ', ' + EnderecoNumero as 'Endereço'
  ,Bairro
  ,Cidade
  ,e.Nome as UF
  ,c.CEP
  ,DtInclusao as 'Data cadastro'
FROM
  tbCliente c (NOLOCK)
  LEFT JOIN tbEstado e (NOLOCK) ON e.idEstado=c.idEstado
WHERE
  DtInclusao BETWEEN @dtInicio AND @dtFim
ORDER BY
  NomeCompleto