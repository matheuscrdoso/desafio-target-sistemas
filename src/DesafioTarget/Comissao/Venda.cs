namespace DesafioTarget.Comissao;

public record Venda(string Vendedor, decimal Valor);

public class ArquivoVendas
{
    public List<Venda> Vendas { get; set; } = new();
}
