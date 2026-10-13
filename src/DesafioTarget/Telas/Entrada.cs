using System.Globalization;

namespace DesafioTarget.Telas;

// Leitura de dados do console com validação, repete a pergunta até vir algo válido.
public static class Entrada
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public static string LerTexto(string pergunta)
    {
        while (true)
        {
            var texto = Ler(pergunta);

            if (!string.IsNullOrWhiteSpace(texto))
                return texto.Trim();

            Erro("Campo obrigatório.");
        }
    }

    public static int LerInteiro(string pergunta)
    {
        while (true)
        {
            if (int.TryParse(Ler(pergunta), out var numero))
                return numero;

            Erro("Digite um número inteiro.");
        }
    }

    public static decimal LerDecimal(string pergunta)
    {
        while (true)
        {
            var texto = Ler(pergunta).Trim();

            // Aceita tanto 1500,90 quanto 1500.90
            var cultura = texto.Contains(',') ? PtBr : CultureInfo.InvariantCulture;

            if (decimal.TryParse(texto, NumberStyles.Number, cultura, out var valor))
                return valor;

            Erro("Valor inválido. Exemplo: 1500,90");
        }
    }

    public static DateTime LerData(string pergunta)
    {
        while (true)
        {
            if (DateTime.TryParseExact(Ler(pergunta).Trim(), "dd/MM/yyyy", PtBr, DateTimeStyles.None, out var data))
                return data;

            Erro("Data inválida. Use o formato dd/mm/aaaa.");
        }
    }

    public static void Erro(string mensagem)
    {
        var corAnterior = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(mensagem);
        Console.ForegroundColor = corAnterior;
    }

    public static void LimparTela()
    {
        // Console.Clear dá erro quando a saída está redirecionada (ex: rodando por script)
        if (!Console.IsOutputRedirected)
            Console.Clear();
    }

    public static void Pausar()
    {
        Console.WriteLine();
        Console.Write("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    private static string Ler(string pergunta)
    {
        Console.Write(pergunta);

        var linha = Console.ReadLine();

        // Entrada encerrada (Ctrl+Z / fim do redirecionamento), não tem mais o que ler
        if (linha is null)
            Environment.Exit(0);

        return linha;
    }
}
