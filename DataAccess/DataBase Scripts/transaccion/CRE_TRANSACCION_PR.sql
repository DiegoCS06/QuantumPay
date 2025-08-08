CREATE PROCEDURE CRE_TRANSACCION_PR
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
    INSERT INTO transaccion (
        IdCuentaBancaria, IdCuentaComercio, IdCuentaCliente,
        Monto, Comision, DescuentoAplicado, Fecha, MetodoPago
    )
    VALUES (
        @P_IdCuentaBancaria, @P_IdCuentaComercio, @P_IdCuentaCliente,
        @P_Monto, @P_Comision, @P_DescuentoAplicado, @P_Fecha, @P_MetodoPago
    );
END