CREATE PROCEDURE UPD_INSTITUCIONBANCARIA_PR
    @P_idInstBancaria INT,
    @P_codigoIdentidad NVARCHAR(30),
    @P_codigoIBAN  NVARCHAR(30),
    @P_cedulaJuridica NVARCHAR(30),
    @P_direccionSedePrincipal NVARCHAR(100),
    @P_telefono INT,
    @P_estadoSolicitud NVARCHAR(20),
    @P_correoElectronico NVARCHAR(50),
    @P_contrasena NVARCHAR(120)
AS
BEGIN
    UPDATE institucionBancaria
    SET 
        codigoIdentidad = @P_codigoIdentidad,
        codigoIBAN = @P_codigoIBAN,
        cedulaJuridica = @P_cedulaJuridica,
        direccionSedePrincipal = @P_direccionSedePrincipal,
        telefono = @P_telefono,
        estadoSolicitud = @P_estadoSolicitud,
        correoElectronico = @P_correoElectronico,
        contrasena = @P_contrasena
    WHERE idInstBancaria = @P_idInstBancaria
END
GO
