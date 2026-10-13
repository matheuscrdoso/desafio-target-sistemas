using DesafioTarget.Estoque;

namespace DesafioTarget.Tests;

public class ControleEstoqueTests
{
    private static ControleEstoque CriarControle()
    {
        return new ControleEstoque(new List<Produto>
        {
            new() { CodigoProduto = 101, DescricaoProduto = "Caneta Azul", Estoque = 150 },
            new() { CodigoProduto = 102, DescricaoProduto = "Caderno Universitário", Estoque = 75 }
        });
    }

    [Fact]
    public void Entrada_SomaNoEstoque()
    {
        var controle = CriarControle();

        var mov = controle.Lancar(101, TipoMovimentacao.Entrada, 50, "Compra de fornecedor");

        Assert.Equal(150, mov.EstoqueAnterior);
        Assert.Equal(200, mov.EstoqueFinal);
        Assert.Equal(200, controle.BuscarProduto(101).Estoque);
    }

    [Fact]
    public void Saida_SubtraiDoEstoque()
    {
        var controle = CriarControle();

        var mov = controle.Lancar(102, TipoMovimentacao.Saida, 25, "Venda");

        Assert.Equal(50, mov.EstoqueFinal);
    }

    [Fact]
    public void CadaMovimentacao_RecebeUmIdDiferente()
    {
        var controle = CriarControle();

        var primeira = controle.Lancar(101, TipoMovimentacao.Entrada, 10, "Compra");
        var segunda = controle.Lancar(102, TipoMovimentacao.Saida, 5, "Venda");
        var terceira = controle.Lancar(101, TipoMovimentacao.Saida, 1, "Avaria");

        Assert.Equal(new[] { 1, 2, 3 }, new[] { primeira.Id, segunda.Id, terceira.Id });
        Assert.Equal(3, controle.Movimentacoes.Count);
    }

    [Fact]
    public void Saida_MaiorQueOSaldo_NaoAlteraOEstoque()
    {
        var controle = CriarControle();

        var erro = Assert.Throws<EstoqueException>(() =>
            controle.Lancar(102, TipoMovimentacao.Saida, 76, "Venda"));

        Assert.Contains("insuficiente", erro.Message);
        Assert.Equal(75, controle.BuscarProduto(102).Estoque);
        Assert.Empty(controle.Movimentacoes);
    }

    [Fact]
    public void Saida_DeTodoOSaldo_ZeraOEstoque()
    {
        var controle = CriarControle();

        var mov = controle.Lancar(102, TipoMovimentacao.Saida, 75, "Venda");

        Assert.Equal(0, mov.EstoqueFinal);
    }

    [Fact]
    public void ProdutoInexistente_LancaErro()
    {
        var controle = CriarControle();

        Assert.Throws<EstoqueException>(() =>
            controle.Lancar(999, TipoMovimentacao.Entrada, 10, "Compra"));
    }

    [Theory]
    [InlineData(0, "Compra")]
    [InlineData(-5, "Compra")]
    [InlineData(10, "   ")]
    public void DadosInvalidos_LancaErro(int quantidade, string descricao)
    {
        var controle = CriarControle();

        Assert.Throws<EstoqueException>(() =>
            controle.Lancar(101, TipoMovimentacao.Entrada, quantidade, descricao));
    }
}
