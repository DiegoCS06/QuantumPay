CREATE PROCEDURE UPD_CUENTACOMERCIO_PR
    @P_IdCuenta INT,
    @P_NombreUsuario NVARCHAR(20),
    @P_Contrasena NVARCHAR(120),
    @P_CedulaJuridica NVARCHAR(20),
    @P_Telefono INT,
    @P_CorreoElectronico NVARCHAR(50),
    @P_Direccion NVARCHAR(100),
    @P_IdComercio INT = NULL
AS
BEGIN
    UPDATE CuentaComercio
    SET
        nombreUsuario = @P_NombreUsuario,
        contrasena = @P_Contrasena,
        cedulaJuridica = @P_CedulaJuridica,
        telefono = @P_Telefono,
        correoElectronico = @P_CorreoElectronico,
        direccion = @P_Direccion,
        IdComercio = @P_IdComercio
    WHERE idCuenta = @P_IdCuenta;
END