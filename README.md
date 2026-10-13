# Desafio Target Sistemas

Resolução do desafio técnico para a vaga de Desenvolvedor(a) de Sistemas Jr.

Fiz em **C# (.NET 8)** como uma aplicação de console com menu, já que é a stack da vaga. Os três exercícios ficam no mesmo programa e cada um tem testes unitários com xUnit. Como extra, deixei na pasta `sql/` os exercícios 1 e 2 resolvidos também em SQL Server.

## Como rodar

Precisa do [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
# rodar o programa
dotnet run --project src/DesafioTarget

# rodar os testes
dotnet test
```

Ao abrir aparece o menu:

```
1 - Comissão dos vendedores
2 - Movimentação de estoque
3 - Cálculo de juros
0 - Sair
```

## Estrutura

```
src/DesafioTarget/
  Dados/          JSONs do enunciado (vendas e estoque)
  Comissao/       exercício 1
  Estoque/        exercício 2
  Juros/          exercício 3
  Telas/          parte de console (menus, leitura e validação do que o usuário digita)
  ArquivoJson.cs  leitura dos arquivos JSON
tests/DesafioTarget.Tests/
sql/
```

Procurei separar a regra de negócio da parte de tela: as classes de `Comissao`, `Estoque` e `Juros` não usam `Console` em nenhum momento, só recebem os dados e devolvem o resultado. Isso deixou os testes bem simples e, se precisar, dá pra reaproveitar essas classes numa API ou em outra interface sem mexer na regra.

## Exercício 1 - Comissão

Lê o `vendas.json`, calcula a comissão de cada venda e agrupa por vendedor.

- abaixo de R$ 100,00: sem comissão
- de R$ 100,00 até R$ 499,99: 1%
- a partir de R$ 500,00: 5%

Resultado com os dados do enunciado:

| Vendedor        | Vendas | Total vendido | Comissão  |
|-----------------|-------:|--------------:|----------:|
| João Silva      | 10     | R$ 10.754,70  | R$ 495,68 |
| Maria Souza     | 9      | R$ 9.874,30   | R$ 465,95 |
| Ana Lima        | 9      | R$ 8.763,95   | R$ 404,98 |
| Carlos Oliveira | 8      | R$ 7.928,35   | R$ 379,37 |

Usei `decimal` em todos os valores de dinheiro (com `double` aparecem aquelas diferenças de centavo). O arredondamento para 2 casas é feito só no total de cada vendedor, e não venda por venda, pra não acumular erro.

## Exercício 2 - Movimentação de estoque

Carrega os produtos do `estoque.json` e permite lançar entradas e saídas. Cada movimentação tem:

- **Id** único e sequencial (1, 2, 3...)
- **Tipo** (entrada ou saída)
- **Descrição** livre pra identificar o motivo (compra de fornecedor, venda, devolução, avaria...)
- quantidade, data/hora, saldo anterior e saldo final

Depois de lançar, o programa mostra o estoque final do produto. Também tem uma opção pra ver o histórico das movimentações feitas.

Validações: produto precisa existir, quantidade maior que zero, descrição obrigatória e não deixa fazer saída maior que o saldo (o estoque nunca fica negativo).

As movimentações ficam em memória enquanto o programa está aberto. O enunciado não pedia persistência, então preferi não gravar por cima do JSON original. No script `sql/02_estoque.sql` tem a versão com banco, que seria como eu faria num sistema real.

## Exercício 3 - Juros

Informa o valor e a data de vencimento, e o programa calcula os juros até a data de hoje com 2,5% ao dia.

Considerei **juros simples sobre o valor original**:

```
dias de atraso = hoje - vencimento   (se não estiver vencido, 0)
juros          = valor x 2,5% x dias de atraso
```

Exemplo: R$ 1.000,00 com 10 dias de atraso = R$ 250,00 de juros, total R$ 1.250,00.

O enunciado fala em "multa de 2,5% ao dia", então entendi como uma taxa diária aplicada sobre o valor original (sem juros compostos). Se o vencimento for hoje ou uma data futura, não há juros.

A data de hoje é passada como parâmetro para a calculadora (a tela usa `DateTime.Today`), assim os testes não dependem do dia em que forem executados.

## Extra: SQL Server

- `sql/01_comissao.sql`: cria a tabela de vendas, insere os dados e calcula a comissão por vendedor com `CASE` e `GROUP BY`.
- `sql/02_estoque.sql`: tabelas de produto e movimentação (o Id é `IDENTITY`) e uma procedure `sp_LancarMovimentacao` que valida o saldo e grava a movimentação dentro de uma transação.

## Se eu tivesse mais tempo

- Persistir as movimentações no SQL Server usando os scripts da pasta `sql/`
- Expor os cálculos numa API REST com ASP.NET Core
- Permitir informar a data base no cálculo de juros, e não só a data de hoje
