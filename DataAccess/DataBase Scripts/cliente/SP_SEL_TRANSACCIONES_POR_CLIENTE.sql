CREATE PROCEDURE dbo.SP_SEL_TRANSACCIONES_POR_CLIENTE
    @ClienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        t.Id,
        t.IdCuentaBancaria,
        t.IdCuentaComercio,
        t.Monto,
        t.Comision,
        t.DescuentoAplicado,
        t.Fecha,
        t.MetodoPago
    FROM dbo.Transaccion AS t
    INNER JOIN dbo.ClienteCuenta AS cc
        ON t.IdCuentaBancaria = cc.Id
    WHERE cc.ClienteId = @ClienteId
    ORDER BY t.Fecha DESC;
END
GO