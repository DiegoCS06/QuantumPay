CREATE PROCEDURE RET_ALL_CUENTACOMERCIO_PR
AS
BEGIN
    SELECT 
        idCuenta, nombreUsuario, contrasena, cedulaJuridica, telefono, correoElectronico, direccion, IdComercio
    FROM CuentaComercio;
END