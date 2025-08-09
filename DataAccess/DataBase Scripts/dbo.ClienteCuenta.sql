CREATE TABLE dbo.ClienteCuenta (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    NumeroCuenta VARCHAR(50) NOT NULL,
    Banco VARCHAR(100) NOT NULL,
    TipoCuenta VARCHAR(50) NOT NULL,
    Saldo DECIMAL(18,2) NOT NULL DEFAULT(0),
    FechaCreacion DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_ClienteCuenta_Cliente
      FOREIGN KEY (ClienteId)
      REFERENCES dbo.cliente(idCliente)
);