namespace DesafioTarget.Estoque;

// Erros de regra de negócio do estoque (saldo insuficiente, produto inexistente etc.)
public class EstoqueException : Exception
{
    public EstoqueException(string mensagem) : base(mensagem)
    {
    }
}
