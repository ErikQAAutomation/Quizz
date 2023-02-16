#language: pt
Funcionalidade: Correios
	Como: Analista de Automação
	Quero: Demonstrar meus conhecimentos
	Para: conseguir uma vaga na B3

Esquema do Cenário: Busca CEP
	Dado Que possuo acesso ao site dos correios
	Quando Procuro pelo CEP <CEP>
	Então Valido endereco < Endereco >
	E localildade <Estado>	

	Exemplos: 
	| CEP       | Endereco               | Estado       |
	| 80700000  | Dados não encontrado   |              |
	| 01013-001 | Rua Quinze de Novembro | São Paulo/SP |
	
	