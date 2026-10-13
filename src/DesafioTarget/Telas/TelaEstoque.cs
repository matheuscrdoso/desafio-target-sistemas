using DesafioTarget.Estoque;

namespace DesafioTarget.Telas;

public class TelaEstoque
{
    private readonly ControleEstoque _controle;

    public TelaEstoque()
    {
        var produtos = ArquivoJson.Ler<ArquivoEstoque>("estoque.json").Estoque;
        _controle = new ControleEstoque(produtos);
    }

    public void Exibir()
    {
        while (true)
        {
            Entrada.LimparTela();
            Console.WriteLine("=== MOVIMENTAÇÃO DE ESTOQUE ===");
            Console.WriteLine();
            ListarProdutos();
            Console.WriteLine();
            Console.WriteLine("1 - Lançar entrada");
            Console.WriteLine("2 - Lançar saída");
            Console.WriteLine("3 - Histórico de movimentações");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            switch (Entrada.LerInteiro("Opção: "))
            {
                case 1:
                    LancarMovimentacao(TipoMovimentacao.Entrada);
                    break;
                case 2:
                    LancarMovimentacao(TipoMovimentacao.Saida);
                    break;
                case 3:
                    ExibirHistorico();
                    break;
                case 0:
                    return;
                default:
                    Entrada.Erro("Opção inválida.");
                    Entrada.Pausar();
                    break;
            }
        }
    }

    private void ListarProdutos()
    {
        Console.WriteLine($"{"Código",-8}{"Produto",-30}{"Estoque",8}");
        Console.WriteLine(new string('-', 46));

        foreach (var p in _controle.Produtos)
            Console.WriteLine($"{p.CodigoProduto,-8}{p.DescricaoProduto,-30}{p.Estoque,8}");
    }

    private void LancarMovimentacao(TipoMovimentacao tipo)
    {
        Console.WriteLine();
        Console.WriteLine(tipo == TipoMovimentacao.Entrada ? "-- Nova entrada --" : "-- Nova saída --");

        try
        {
            // Já valida o produto aqui pra não fazer a pessoa digitar tudo à toa
            var produto = _controle.BuscarProduto(Entrada.LerInteiro("Código do produto: "));
            Console.WriteLine($"Produto: {produto.DescricaoProduto} (saldo atual: {produto.Estoque})");

            var quantidade = Entrada.LerInteiro("Quantidade: ");
            var descricao = Entrada.LerTexto("Descrição (ex: compra de fornecedor, venda, devolução): ");

            var mov = _controle.Lancar(produto.CodigoProduto, tipo, quantidade, descricao);

            Console.WriteLine();
            Console.WriteLine($"Movimentação nº {mov.Id} registrada.");
            Console.WriteLine($"{produto.DescricaoProduto}: {mov.EstoqueAnterior} -> {mov.EstoqueFinal}");
            Console.WriteLine($"Estoque final do produto: {mov.EstoqueFinal}");
        }
        catch (EstoqueException ex)
        {
            Entrada.Erro(ex.Message);
        }

        Entrada.Pausar();
    }

    private void ExibirHistorico()
    {
        Console.WriteLine();

        if (_controle.Movimentacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma movimentação lançada ainda.");
            Entrada.Pausar();
            return;
        }

        Console.WriteLine($"{"Nº",-5}{"Data",-18}{"Tipo",-9}{"Produto",-9}{"Qtd",6}{"Saldo",8}  Descrição");
        Console.WriteLine(new string('-', 75));

        foreach (var m in _controle.Movimentacoes)
        {
            var tipo = m.Tipo == TipoMovimentacao.Entrada ? "Entrada" : "Saída";

            Console.WriteLine($"{m.Id,-5}{m.Data,-18:dd/MM/yyyy HH:mm}{tipo,-9}{m.CodigoProduto,-9}" +
                              $"{m.Quantidade,6}{m.EstoqueFinal,8}  {m.Descricao}");
        }

        Entrada.Pausar();
    }
}
