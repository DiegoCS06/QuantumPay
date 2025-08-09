CREATE PROCEDURE RET_TRANSACCION_BY_COMERCIO_PR
    @P_IdCuentaComercio INT
AS
BEGIN
    SELECT 
        t.*,
        c.Nombre AS NombreCliente,
        ib.codigoIdentidad AS CodigoIdentidadInstitucionBancaria
    FROM transaccion t
    INNER JOIN Cliente c ON t.IdCuentaCliente = c.IdCliente
    INNER JOIN InstitucionBancaria ib ON t.IdCuentaBancaria = ib.idInstBancaria
    WHERE t.IdCuentaComercio = @P_IdCuentaComercio;
END