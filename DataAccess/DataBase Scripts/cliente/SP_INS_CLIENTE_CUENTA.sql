CREATE PROCEDURE dbo.SP_INS_CLIENTE_CUENTA
    @ClienteId     INT,
    @NumeroCuenta  VARCHAR(50),
    @Banco         VARCHAR(100),
    @TipoCuenta    VARCHAR(50),
    @Saldo         DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.ClienteCuenta
        (ClienteId, NumeroCuenta, Banco, TipoCuenta, Saldo)
    VALUES
        (@ClienteId, @NumeroCuenta, @Banco, @TipoCuenta, @Saldo);

    SELECT SCOPE_IDENTITY() AS NewId;
END
GO