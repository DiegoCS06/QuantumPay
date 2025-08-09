CREATE PROCEDURE dbo.SP_UPD_CLIENTE_CUENTA
    @Id            INT,
    @NumeroCuenta  VARCHAR(50),
    @Banco         VARCHAR(100),
    @TipoCuenta    VARCHAR(50),
    @Saldo         DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.ClienteCuenta
       SET NumeroCuenta = @NumeroCuenta,
           Banco        = @Banco,
           TipoCuenta   = @TipoCuenta,
           Saldo        = @Saldo
     WHERE Id = @Id;
END
GO