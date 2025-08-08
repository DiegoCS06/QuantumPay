CREATE PROCEDURE RET_TRANSACCION_BY_BANCO_PR
    @P_IdCuentaBancaria INT
AS
BEGIN
    SELECT 
        t.*,
        c.Nombre AS NombreCliente,
        ib.codigoIdentidad AS CodigoIdentidadInstitucionBancaria
    FROM transaccion t
    INNER JOIN Cliente c ON t.IdCuentaCliente = c.IdCliente
    INNER JOIN InstitucionBancaria ib ON t.IdCuentaBancaria = ib.idInstBancaria
    WHERE t.IdCuentaBancaria = @P_IdCuentaBancaria;
END