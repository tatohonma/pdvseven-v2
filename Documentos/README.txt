Resumo da Estrutura do Projeto

a7D.PDV.sln
	Pastas
		Aplicações -> Projetos finais que serão instalador no cliente
		Fiscal -> SAT e NFCe
		Infra -> Instalador, SharedAssemblyInfo, Readme, Componentes, EF, e outros projetos comuns a quase todas aplicações
		Integrações -> Projeto das integraçoes usadas no Caixa e Integrador

	* no arquivo 'SharedAssemblyInfo.cs' comum a todos projetos há um histórico das principais alterações

	2 tipos de "Shared Projects"
		* Quando usado em 'plataformas' diferentes ou sem a necessidade de gerar nova DLL (.Net 4.6.1, .Net Core, Windows Form, WPF)
			- Iaago-BigData, a7D.PDV.Atualizacao.Shared
		* Usados como DLL comum
			- a7D.PDV.BigData.Shared, a7D.PDV.Ativacao.Shared
		
	=> Usa Shared Project de Ativações
	=> Usa Shared Project de Iaago-BigData

Ativacoes.sln
	Sistema de Controle de Licenças
	Publicação no Azure
		a7D.PDV.Ativacao.API => apipdvseven (https://apipdvseven.azurewebsites.net) 
			WebAPI do sistema de ativações, mensagens, envio de erros
		a7D.PDV.Ativacao.UIWeb2 => ativacoespdvseven (http://ativacoes.pdvseven.com.br) 
			Sistema atual de Ativações
		Descontinuado\a7D.PDV.Ativacao.WS => wsativacaopdvseven (http://ativacao.pdvseven.com.br) 
			Validador de licenças para cliente anterior a versão 2.13

Iaago-BigData.sln
	ChatBot e BigData API

Metricas
	Entender Applications Insights
    GA / Firebird : usar os indicadores para algo

O instaldor é compilado pelo arquivo "instalador_PDVSeven.nsi" e o instalador do compilador é o "nsis-2.51-setup.exe"
Após instalado o compilador NSIS, clicar no script com botão direito e compilar
Sempre na troca de versão editar o numero da versão do instalador no arquivo ".nsi"
Para criar o atualizador!
Use o menu de contexto do 7Zip e descompacte o instalador, compacte o conteudo como um ZIP e coloque com o nome correto via FTP
em a7D.PDV.Ativacao.UIWeb2\Scripts\ng\Controllers\mensagens-controller.js está o links das possiveis mensagens

MANTER NO MAXIMO 2 Versões ativas em 'Branchs' diferente no git... Em produção, e proxima versão, para evitar perda de código e conclitos de merge!
A lista detalhada, planejamento, histórico de versões e dívidas de software estão no RoadMap

Roadamp
	https://docs.google.com/document/d/1T0UqhKqQIKcYuXewtrOMA7A7-DB7zj9tJiJovSPlmY8

Roteiro de Testes
	https://docs.google.com/spreadsheets/d/1s_SS8NohYRY1fw3YN-cfaNs0LsCEr4xFY_21FtQVr84

Lista de Funcionalidades:
	https://docs.google.com/document/d/1btwX4zvexmPNwrdQqIQ2QE3I8NxmImUHdaz5Os7Zr3A

Versões dos Clientes
http://apipdvseven.azurewebsites.net/clientes.ashx ?cmd=[clear|erro|all]
