CREATE PROCEDURE CRE_CUENTACOMERCIO_PR
    @P_NombreUsuario NVARCHAR(20),
    @P_Contrasena NVARCHAR(120),
    @P_CedulaJuridica NVARCHAR(20),
    @P_Telefono INT,
    @P_CorreoElectronico NVARCHAR(50),
    @P_Direccion NVARCHAR(100),
    @P_IdComercio INT = NULL
AS
BEGIN
    INSERT INTO CuentaComercio (
        nombreUsuario, contrasena, cedulaJuridica, telefono, correoElectronico, direccion, IdComercio
    )
    VALUES (
        @P_NombreUsuario, @P_Contrasena, @P_CedulaJuridica, @P_Telefono, @P_CorreoElectronico, @P_Direccion, @P_IdComercio
    );
END