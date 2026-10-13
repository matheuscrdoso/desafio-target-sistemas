-- Exercício 2 em SQL Server: tabelas + procedure para lançar a movimentação

IF OBJECT_ID('dbo.MovimentacaoEstoque', 'U') IS NOT NULL
    DROP TABLE dbo.MovimentacaoEstoque;

IF OBJECT_ID('dbo.Produto', 'U') IS NOT NULL
    DROP TABLE dbo.Produto;

CREATE TABLE dbo.Produto (
    CodigoProduto     INT          PRIMARY KEY,
    DescricaoProduto  NVARCHAR(100) NOT NULL,
    Estoque           INT          NOT NULL CHECK (Estoque >= 0)
);

CREATE TABLE dbo.MovimentacaoEstoque (
    Id               INT IDENTITY(1,1) PRIMARY KEY,   -- número identificador único
    CodigoProduto    INT          NOT NULL REFERENCES dbo.Produto (CodigoProduto),
    Tipo             CHAR(1)      NOT NULL CHECK (Tipo IN ('E', 'S')),  -- E = entrada, S = saída
    Quantidade       INT          NOT NULL CHECK (Quantidade > 0),
    Descricao        NVARCHAR(200) NOT NULL,
    EstoqueAnterior  INT          NOT NULL,
    EstoqueFinal     INT          NOT NULL,
    DataMovimentacao DATETIME     NOT NULL DEFAULT GETDATE()
);

INSERT INTO dbo.Produto (CodigoProduto, DescricaoProduto, Estoque) VALUES
    (101, N'Caneta Azul', 150),
    (102, N'Caderno Universitário', 75),
    (103, N'Borracha Branca', 200),
    (104, N'Lápis Preto HB', 320),
    (105, N'Marcador de Texto Amarelo', 90);
GO

CREATE OR ALTER PROCEDURE dbo.sp_LancarMovimentacao
    @CodigoProduto INT,
    @Tipo          CHAR(1),
    @Quantidade    INT,
    @Descricao     NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @EstoqueAnterior INT, @EstoqueFinal INT;

    BEGIN TRANSACTION;

    -- UPDLOCK pra ninguém mexer no saldo do produto enquanto a movimentação é lançada
    SELECT @EstoqueAnterior = Estoque
    FROM dbo.Produto WITH (UPDLOCK, ROWLOCK)
    WHERE CodigoProduto = @CodigoProduto;

    IF @EstoqueAnterior IS NULL
        THROW 50001, N'Produto não encontrado.', 1;

    IF @Tipo = 'S' AND @Quantidade > @EstoqueAnterior
        THROW 50002, N'Estoque insuficiente para a saída.', 1;

    SET @EstoqueFinal = @EstoqueAnterior + CASE WHEN @Tipo = 'E' THEN @Quantidade ELSE -@Quantidade END;

    UPDATE dbo.Produto
    SET Estoque = @EstoqueFinal
    WHERE CodigoProduto = @CodigoProduto;

    INSERT INTO dbo.MovimentacaoEstoque
        (CodigoProduto, Tipo, Quantidade, Descricao, EstoqueAnterior, EstoqueFinal)
    VALUES
        (@CodigoProduto, @Tipo, @Quantidade, @Descricao, @EstoqueAnterior, @EstoqueFinal);

    COMMIT TRANSACTION;

    SELECT SCOPE_IDENTITY() AS IdMovimentacao, @EstoqueFinal AS EstoqueFinal;
END
GO

-- Exemplos
EXEC dbo.sp_LancarMovimentacao 101, 'E', 50, N'Compra de fornecedor';
EXEC dbo.sp_LancarMovimentacao 102, 'S', 25, N'Venda';

SELECT * FROM dbo.MovimentacaoEstoque ORDER BY Id;
