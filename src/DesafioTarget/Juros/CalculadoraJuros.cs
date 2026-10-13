namespace DesafioTarget.Juros;

public record ResultadoJuros(decimal ValorOriginal, int DiasAtraso, decimal Juros, decimal ValorAtualizado);

public static class CalculadoraJuros
{
    public const decimal TaxaDiaria = 0.025m;

    // "hoje" vem por parâmetro para dar pra testar com datas fixas.
    public static ResultadoJuros Calcular(decimal valor, DateTime vencimento, DateTime hoje)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor precisa ser maior que zero.", nameof(valor));

        var diasAtraso = Math.Max(0, (hoje.Date - vencimento.Date).Days);

        // Juros simples: 2,5% sobre o valor original para cada dia de atraso
        var juros = Math.Round(valor * TaxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);

        return new ResultadoJuros(valor, diasAtraso, juros, valor + juros);
    }
}
