using System.Globalization;
using System.Text;
using DesafioTarget.Telas;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

// O estoque fica em memória enquanto o programa estiver aberto,
// por isso a tela é criada uma vez só e reaproveitada.
var telaEstoque = new TelaEstoque();

while (true)
{
    Entrada.LimparTela();
    Console.WriteLine("=== DESAFIO TARGET SISTEMAS ===");
    Console.WriteLine();
    Console.WriteLine("1 - Comissão dos vendedores");
    Console.WriteLine("2 - Movimentação de estoque");
    Console.WriteLine("3 - Cálculo de juros");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    switch (Entrada.LerInteiro("Escolha uma opção: "))
    {
        case 1:
            TelaComissao.Exibir();
            break;
        case 2:
            telaEstoque.Exibir();
            break;
        case 3:
            TelaJuros.Exibir();
            break;
        case 0:
            return;
        default:
            Entrada.Erro("Opção inválida.");
            Entrada.Pausar();
            break;
    }
}
