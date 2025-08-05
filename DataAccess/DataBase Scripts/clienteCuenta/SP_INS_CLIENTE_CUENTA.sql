CREATE PROCEDURE SP_INS_CLIENTE_CUENTA
    @ClienteId INT,
    @NumeroCuenta VARCHAR(30),
    @Banco VARCHAR(100),
    @TipoCuenta VARCHAR(50),
    @Saldo DECIMAL(18,2)
AS
BEGIN
    INSERT INTO dbo.ClienteCuenta (ClienteId, NumeroCuenta, Banco, TipoCuenta, Saldo)
    VALUES (@ClienteId, @NumeroCuenta, @Banco, @TipoCuenta, @Saldo);
END
GO
