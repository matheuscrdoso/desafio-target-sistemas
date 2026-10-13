using DesafioTarget.Juros;

namespace DesafioTarget.Telas;

public static class TelaJuros
{
    public static void Exibir()
    {
        Entrada.LimparTela();
        Console.WriteLine("=== CÁLCULO DE JUROS POR ATRASO ===");
        Console.WriteLine();
        Console.WriteLine($"Taxa: {CalculadoraJuros.TaxaDiaria:P1} ao dia sobre o valor original.");
        Console.WriteLine();

        decimal valor;
        while ((valor = Entrada.LerDecimal("Valor (R$): ")) <= 0)
            Entrada.Erro("O valor precisa ser maior que zero.");

        var vencimento = Entrada.LerData("Data de vencimento (dd/mm/aaaa): ");
        var hoje = DateTime.Today;

        var resultado = CalculadoraJuros.Calcular(valor, vencimento, hoje);

        Console.WriteLine();
        Console.WriteLine($"Data de hoje:     {hoje:dd/MM/yyyy}");
        Console.WriteLine($"Vencimento:       {vencimento:dd/MM/yyyy}");
        Console.WriteLine($"Dias em atraso:   {resultado.DiasAtraso}");
        Console.WriteLine($"Valor original:   {resultado.ValorOriginal:C2}");
        Console.WriteLine($"Juros:            {resultado.Juros:C2}");
        Console.WriteLine($"Valor atualizado: {resultado.ValorAtualizado:C2}");

        if (resultado.DiasAtraso == 0)
        {
            Console.WriteLine();
            Console.WriteLine("Conta dentro do prazo, não há juros a cobrar.");
        }

        Entrada.Pausar();
    }
}
