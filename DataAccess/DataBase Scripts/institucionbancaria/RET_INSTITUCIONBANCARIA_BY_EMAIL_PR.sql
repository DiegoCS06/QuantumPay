CREATE PROCEDURE RET_INSTITUCIONBANCARIA_BY_EMAIL_PR
    @P_correoElectronico NVARCHAR(50)
AS
BEGIN
    SELECT 
        idInstBancaria,
        codigoIdentidad,
        codigoIBAN,
        cedulaJuridica,
        direccionSedePrincipal,
        telefono,
        estadoSolicitud,
        correoElectronico,
        contrasena
    FROM institucionBancaria
    WHERE correoElectronico = @P_correoElectronico
END
GO
