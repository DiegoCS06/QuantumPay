ALTER TABLE dbo.Transaccion ADD IBAN VARCHAR(30) NULL;
ALTER TABLE dbo.Transaccion ADD IdCuentaCliente INT NULL;
GO

IF OBJECT_ID('dbo.SP_INS_TRANSACCION') IS NOT NULL
    DROP PROCEDURE dbo.SP_INS_TRANSACCION;
GO
CREATE PROCEDURE dbo.SP_INS_TRANSACCION
    @P_IdCuentaCliente INT,
    @P_IdCuentaBancaria INT,
    @P_IBAN VARCHAR(30),
    @P_IdCuentaComercio INT,
    @P_Monto DECIMAL(18,2),
    @P_Comision DECIMAL(18,2),
    @P_DescuentoAplicado DECIMAL(18,2),
    @P_Fecha DATETIME2,
    @P_MetodoPago VARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.Transaccion
        (IdCuentaCliente, IdCuentaBancaria, IBAN, IdCuentaComercio, Monto, Comision, DescuentoAplicado, Fecha, MetodoPago)
    VALUES
        (@P_IdCuentaCliente, @P_IdCuentaBancaria, @P_IBAN, @P_IdCuentaComercio, @P_Monto, @P_Comision, @P_DescuentoAplicado, @P_Fecha, @P_MetodoPago);
END
GO

IF OBJECT_ID('dbo.SP_SEL_TRANSACCIONES_POR_CLIENTE') IS NOT NULL
    DROP PROCEDURE dbo.SP_SEL_TRANSACCIONES_POR_CLIENTE;
GO
CREATE PROCEDURE dbo.SP_SEL_TRANSACCIONES_POR_CLIENTE
    @ClienteId INT
AS
BEGIN
    SELECT t.Id,
           t.IdCuentaCliente,
           t.IdCuentaBancaria,
           t.IBAN,
           t.IdCuentaComercio,
           t.Monto,
           t.Comision,
           t.DescuentoAplicado,
           t.Fecha,
           t.MetodoPago
    FROM dbo.Transaccion t
        INNER JOIN dbo.ClienteCuenta cc ON t.IdCuentaBancaria = cc.Id
    WHERE cc.ClienteId = @ClienteId
    ORDER BY t.Fecha DESC;
END
GO
