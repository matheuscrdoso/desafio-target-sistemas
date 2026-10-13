using DesafioTarget.Comissao;

namespace DesafioTarget.Telas;

public static class TelaComissao
{
    public static void Exibir()
    {
        Entrada.LimparTela();
        Console.WriteLine("=== COMISSÃO DOS VENDEDORES ===");
        Console.WriteLine();
        Console.WriteLine("Regra: abaixo de R$ 100,00 sem comissão | abaixo de R$ 500,00 = 1% | a partir de R$ 500,00 = 5%");
        Console.WriteLine();

        var vendas = ArquivoJson.Ler<ArquivoVendas>("vendas.json").Vendas;
        var comissoes = CalculadoraComissao.CalcularPorVendedor(vendas);

        Console.WriteLine($"{"Vendedor",-20}{"Vendas",8}{"Total vendido",18}{"Comissão",15}");
        Console.WriteLine(new string('-', 61));

        foreach (var c in comissoes)
            Console.WriteLine($"{c.Vendedor,-20}{c.QuantidadeVendas,8}{c.TotalVendido,18:C2}{c.Comissao,15:C2}");

        Console.WriteLine(new string('-', 61));
        Console.WriteLine($"{"Total",-20}{comissoes.Sum(c => c.QuantidadeVendas),8}" +
                          $"{comissoes.Sum(c => c.TotalVendido),18:C2}{comissoes.Sum(c => c.Comissao),15:C2}");

        Entrada.Pausar();
    }
}
