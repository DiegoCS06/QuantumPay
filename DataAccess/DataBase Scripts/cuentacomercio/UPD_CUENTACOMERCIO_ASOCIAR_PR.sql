CREATE PROCEDURE UPD_CUENTACOMERCIO_ASOCIAR_PR
    @cuentaId INT,
    @comercioId INT
AS
BEGIN
    UPDATE CuentaComercio
    SET IdComercio = @comercioId
    WHERE idCuenta = @cuentaId
END