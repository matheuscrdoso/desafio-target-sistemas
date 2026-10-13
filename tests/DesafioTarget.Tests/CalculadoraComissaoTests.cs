using DesafioTarget.Comissao;

namespace DesafioTarget.Tests;

public class CalculadoraComissaoTests
{
    [Theory]
    [InlineData(50, 0)]
    [InlineData(99.99, 0)]
    [InlineData(100, 0.01)]
    [InlineData(499.99, 0.01)]
    [InlineData(500, 0.05)]
    [InlineData(2100.40, 0.05)]
    public void PercentualPara_RespeitaAsFaixas(decimal valor, decimal percentualEsperado)
    {
        Assert.Equal(percentualEsperado, CalculadoraComissao.PercentualPara(valor));
    }

    [Fact]
    public void ComissaoDaVenda_AbaixoDeCem_NaoGeraComissao()
    {
        Assert.Equal(0m, CalculadoraComissao.ComissaoDaVenda(90.75m));
    }

    [Fact]
    public void CalcularPorVendedor_SomaAsComissoesDeCadaVendedor()
    {
        var vendas = new List<Venda>
        {
            new("Fulano", 1000m),  // 5%  = 50,00
            new("Fulano", 200m),   // 1%  =  2,00
            new("Fulano", 80m),    // 0%  =  0,00
            new("Ciclano", 500m)   // 5%  = 25,00
        };

        var resultado = CalculadoraComissao.CalcularPorVendedor(vendas);

        var fulano = resultado.Single(r => r.Vendedor == "Fulano");
        Assert.Equal(3, fulano.QuantidadeVendas);
        Assert.Equal(1280m, fulano.TotalVendido);
        Assert.Equal(52m, fulano.Comissao);

        Assert.Equal(25m, resultado.Single(r => r.Vendedor == "Ciclano").Comissao);
    }

    [Theory]
    [InlineData("João Silva", 495.68)]
    [InlineData("Maria Souza", 465.95)]
    [InlineData("Carlos Oliveira", 379.37)]
    [InlineData("Ana Lima", 404.98)]
    public void CalcularPorVendedor_ComOsDadosDoDesafio(string vendedor, decimal comissaoEsperada)
    {
        var vendas = ArquivoJson.Ler<ArquivoVendas>("vendas.json").Vendas;

        var resultado = CalculadoraComissao.CalcularPorVendedor(vendas);

        Assert.Equal(comissaoEsperada, resultado.Single(r => r.Vendedor == vendedor).Comissao);
    }
}
