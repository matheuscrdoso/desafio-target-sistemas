using System.Text.Json;

namespace DesafioTarget;

public static class ArquivoJson
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T Ler<T>(string nomeArquivo)
    {
        var caminho = Path.Combine(AppContext.BaseDirectory, "Dados", nomeArquivo);

        if (!File.Exists(caminho))
            throw new FileNotFoundException($"Arquivo de dados não encontrado: {caminho}");

        var conteudo = File.ReadAllText(caminho);

        return JsonSerializer.Deserialize<T>(conteudo, Opcoes)
               ?? throw new InvalidDataException($"Não foi possível ler o arquivo {nomeArquivo}.");
    }
}
