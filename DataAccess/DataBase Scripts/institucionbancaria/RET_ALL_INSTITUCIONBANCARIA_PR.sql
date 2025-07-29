CREATE PROCEDURE RET_ALL_INSTITUCIONBANCARIA_PR
AS
BEGIN
    SELECT 
        idInstBancaria,
        codigoIdentidad,
        cedulaJuridica,
        direccionSedePrincipal,
        telefono,
        estadoSolicitud,
        correoElectronico,
        contrasena
    FROM institucionBancaria
END
GO
