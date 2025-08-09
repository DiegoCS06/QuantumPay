CREATE PROCEDURE RET_TRANSACCION_BY_CLIENTE_PR
    @P_IdCuentaCliente INT
AS
BEGIN
    SELECT 
        t.*,
        c.Nombre AS NombreCliente,
        ib.codigoIdentidad AS CodigoIdentidadInstitucionBancaria
    FROM transaccion t
    INNER JOIN Cliente c ON t.IdCuentaCliente = c.IdCliente
    INNER JOIN InstitucionBancaria ib ON t.IdCuentaBancaria = ib.idInstBancaria
    WHERE t.IdCuentaCliente = @P_IdCuentaCliente;
END