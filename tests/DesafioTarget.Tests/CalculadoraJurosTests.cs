using DesafioTarget.Juros;

namespace DesafioTarget.Tests;

public class CalculadoraJurosTests
{
    private static readonly DateTime Hoje = new(2026, 10, 13);

    [Fact]
    public void DezDiasDeAtraso_Cobra25PorCento()
    {
        var resultado = CalculadoraJuros.Calcular(1000m, Hoje.AddDays(-10), Hoje);

        Assert.Equal(10, resultado.DiasAtraso);
        Assert.Equal(250m, resultado.Juros);
        Assert.Equal(1250m, resultado.ValorAtualizado);
    }

    [Fact]
    public void UmDiaDeAtraso()
    {
        var resultado = CalculadoraJuros.Calcular(150.50m, Hoje.AddDays(-1), Hoje);

        Assert.Equal(1, resultado.DiasAtraso);
        Assert.Equal(3.76m, resultado.Juros); // 150,50 * 2,5% = 3,7625
    }

    [Fact]
    public void VencendoHoje_NaoTemJuros()
    {
        var resultado = CalculadoraJuros.Calcular(500m, Hoje, Hoje);

        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.Juros);
        Assert.Equal(500m, resultado.ValorAtualizado);
    }

    [Fact]
    public void VencimentoFuturo_NaoTemJuros()
    {
        var resultado = CalculadoraJuros.Calcular(500m, Hoje.AddDays(15), Hoje);

        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.Juros);
    }

    [Fact]
    public void IgnoraOHorarioDasDatas()
    {
        var vencimento = new DateTime(2026, 10, 12, 23, 59, 0);
        var agora = new DateTime(2026, 10, 13, 0, 1, 0);

        Assert.Equal(1, CalculadoraJuros.Calcular(100m, vencimento, agora).DiasAtraso);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void ValorInvalido_LancaErro(decimal valor)
    {
        Assert.Throws<ArgumentException>(() => CalculadoraJuros.Calcular(valor, Hoje, Hoje));
    }
}
