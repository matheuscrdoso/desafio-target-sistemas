namespace DesafioTarget.Comissao;

public static class CalculadoraComissao
{
    private const decimal ValorMinimoComissao = 100m;
    private const decimal ValorFaixaMaior = 500m;

    private const decimal PercentualFaixaMenor = 0.01m;
    private const decimal PercentualFaixaMaior = 0.05m;

    public static decimal PercentualPara(decimal valorVenda)
    {
        if (valorVenda < ValorMinimoComissao)
            return 0m;

        if (valorVenda < ValorFaixaMaior)
            return PercentualFaixaMenor;

        return PercentualFaixaMaior;
    }

    public static decimal ComissaoDaVenda(decimal valorVenda)
    {
        return valorVenda * PercentualPara(valorVenda);
    }

    public static List<ComissaoVendedor> CalcularPorVendedor(IEnumerable<Venda> vendas)
    {
        // O arredondamento é feito só no total de cada vendedor, para não acumular
        // diferença de centavos arredondando venda por venda.
        return vendas
            .GroupBy(v => v.Vendedor)
            .Select(grupo => new ComissaoVendedor(
                grupo.Key,
                grupo.Count(),
                grupo.Sum(v => v.Valor),
                Math.Round(grupo.Sum(v => ComissaoDaVenda(v.Valor)), 2, MidpointRounding.AwayFromZero)))
            .OrderByDescending(c => c.Comissao)
            .ToList();
    }
}
