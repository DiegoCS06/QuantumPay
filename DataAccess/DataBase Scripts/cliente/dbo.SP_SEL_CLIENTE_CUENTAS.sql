CREATE PROCEDURE dbo.SP_SEL_CLIENTE_CUENTAS
    @ClienteId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        Id,
        ClienteId,
        NumeroCuenta,
        Banco,
        TipoCuenta,
        Saldo,
        FechaCreacion
      FROM dbo.ClienteCuenta
     WHERE ClienteId = @ClienteId
     ORDER BY FechaCreacion DESC;
END
GO