-- Exercício 1 resolvido direto no banco (SQL Server)

IF OBJECT_ID('dbo.Venda', 'U') IS NOT NULL
    DROP TABLE dbo.Venda;

CREATE TABLE dbo.Venda (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Vendedor  NVARCHAR(100)  NOT NULL,
    Valor     DECIMAL(18,2) NOT NULL
);

INSERT INTO dbo.Venda (Vendedor, Valor) VALUES
    (N'João Silva', 1200.50), (N'João Silva', 950.75), (N'João Silva', 1800.00), (N'João Silva', 1400.30),
    (N'João Silva', 1100.90), (N'João Silva', 1550.00), (N'João Silva', 1700.80), (N'João Silva', 250.30),
    (N'João Silva', 480.75), (N'João Silva', 320.40),

    (N'Maria Souza', 2100.40), (N'Maria Souza', 1350.60), (N'Maria Souza', 950.20), (N'Maria Souza', 1600.75),
    (N'Maria Souza', 1750.00), (N'Maria Souza', 1450.90), (N'Maria Souza', 400.50), (N'Maria Souza', 180.20),
    (N'Maria Souza', 90.75),

    (N'Carlos Oliveira', 800.50), (N'Carlos Oliveira', 1200.00), (N'Carlos Oliveira', 1950.30),
    (N'Carlos Oliveira', 1750.80), (N'Carlos Oliveira', 1300.60), (N'Carlos Oliveira', 300.40),
    (N'Carlos Oliveira', 500.00), (N'Carlos Oliveira', 125.75),

    (N'Ana Lima', 1000.00), (N'Ana Lima', 1100.50), (N'Ana Lima', 1250.75), (N'Ana Lima', 1400.20),
    (N'Ana Lima', 1550.90), (N'Ana Lima', 1650.00), (N'Ana Lima', 75.30), (N'Ana Lima', 420.90),
    (N'Ana Lima', 315.40);

SELECT
    Vendedor,
    COUNT(*)                     AS QtdVendas,
    SUM(Valor)                   AS TotalVendido,
    ROUND(SUM(Valor * CASE
                          WHEN Valor < 100 THEN 0
                          WHEN Valor < 500 THEN 0.01
                          ELSE 0.05
                      END), 2)   AS Comissao
FROM dbo.Venda
GROUP BY Vendedor
ORDER BY Comissao DESC;
