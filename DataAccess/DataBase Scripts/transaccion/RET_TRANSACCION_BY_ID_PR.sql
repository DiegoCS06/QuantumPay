CREATE PROCEDURE RET_TRANSACCION_BY_ID_PR
    @P_Id INT
AS
BEGIN
    SELECT 
        t.*,
        c.Nombre AS NombreCliente,
        ib.codigoIdentidad AS CodigoIdentidadInstitucionBancaria
    FROM transaccion t
    INNER JOIN Cliente c ON t.IdCuentaCliente = c.IdCliente
    INNER JOIN InstitucionBancaria ib ON t.IdCuentaBancaria = ib.idInstBancaria
    WHERE t.Id = @P_Id;
END