CREATE TABLE dbo.Transaccion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdCuentaBancaria INT NOT NULL,
    IdCuentaComercio INT NOT NULL,
    IdCuentaCliente INT NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    Comision DECIMAL(18,2) NOT NULL,
    DescuentoAplicado DECIMAL(18,2) NOT NULL,
    Fecha DATETIME2 NOT NULL,
    MetodoPago VARCHAR(50) NOT NULL
);