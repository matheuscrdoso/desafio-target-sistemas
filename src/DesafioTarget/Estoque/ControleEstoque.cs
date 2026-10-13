namespace DesafioTarget.Estoque;

public class ControleEstoque
{
    private readonly Dictionary<int, Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = new();
    private int _ultimoId;

    public ControleEstoque(IEnumerable<Produto> produtos)
    {
        _produtos = produtos.ToDictionary(p => p.CodigoProduto);
    }

    public IEnumerable<Produto> Produtos => _produtos.Values.OrderBy(p => p.CodigoProduto);

    public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

    public Produto BuscarProduto(int codigoProduto)
    {
        if (!_produtos.TryGetValue(codigoProduto, out var produto))
            throw new EstoqueException($"Produto {codigoProduto} não encontrado.");

        return produto;
    }

    public Movimentacao Lancar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        var produto = BuscarProduto(codigoProduto);

        if (quantidade <= 0)
            throw new EstoqueException("A quantidade precisa ser maior que zero.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new EstoqueException("Informe uma descrição para a movimentação.");

        if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
            throw new EstoqueException(
                $"Estoque insuficiente. Saldo atual de {produto.DescricaoProduto}: {produto.Estoque}.");

        var estoqueAnterior = produto.Estoque;

        produto.Estoque += tipo == TipoMovimentacao.Entrada ? quantidade : -quantidade;

        var movimentacao = new Movimentacao(
            ++_ultimoId,
            produto.CodigoProduto,
            tipo,
            quantidade,
            descricao.Trim(),
            DateTime.Now,
            estoqueAnterior,
            produto.Estoque);

        _movimentacoes.Add(movimentacao);

        return movimentacao;
    }
}
