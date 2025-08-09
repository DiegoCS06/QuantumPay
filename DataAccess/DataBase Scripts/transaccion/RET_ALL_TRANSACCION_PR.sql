CREATE PROCEDURE RET_ALL_TRANSACCION_PR
AS
BEGIN
    SELECT 
        t.*,
        c.Nombre AS NombreCliente,
        ib.codigoIdentidad AS CodigoIdentidadInstitucionBancaria
    FROM transaccion t
    INNER JOIN Cliente c ON t.IdCuentaCliente = c.IdCliente
    INNER JOIN InstitucionBancaria ib ON t.IdCuentaBancaria = ib.idInstBancaria;
END
