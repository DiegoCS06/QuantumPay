CREATE PROCEDURE RET_INSTITUCIONBANCARIA_BY_CODIGOIDENTIDAD_PR
    @P_codigoIdentidad NVARCHAR(30)
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
    WHERE codigoIdentidad = @P_codigoIdentidad
END
GO
