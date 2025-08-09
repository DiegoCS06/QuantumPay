CREATE PROCEDURE UPD_TRANSACCION_PR
    @P_Id INT,
    @P_IdCuentaBancaria INT,
    @P_IdCuentaComercio INT,
    @P_IdCuentaCliente INT,
    @P_Monto DECIMAL(18,2),
    @P_Comision DECIMAL(18,2),
    @P_DescuentoAplicado DECIMAL(18,2),
    @P_Fecha DATETIME2,
    @P_MetodoPago VARCHAR(50)
AS
BEGIN
    UPDATE transaccion
    SET
        IdCuentaBancaria = @P_IdCuentaBancaria,
        IdCuentaComercio = @P_IdCuentaComercio,
        IdCuentaCliente = @P_IdCuentaCliente,
        Monto = @P_Monto,
        Comision = @P_Comision,
        DescuentoAplicado = @P_DescuentoAplicado,
        Fecha = @P_Fecha,
        MetodoPago = @P_MetodoPago
    WHERE Id = @P_Id;
END
