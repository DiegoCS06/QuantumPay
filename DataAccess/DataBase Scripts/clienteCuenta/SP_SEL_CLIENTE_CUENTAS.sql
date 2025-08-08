CREATE PROCEDURE SP_SEL_CLIENTE_CUENTAS
    @ClienteId INT
AS
BEGIN
    SELECT Id,
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
