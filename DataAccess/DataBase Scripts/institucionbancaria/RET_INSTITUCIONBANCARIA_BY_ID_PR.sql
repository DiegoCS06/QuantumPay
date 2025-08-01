CREATE PROCEDURE RET_INSTITUCIONBANCARIA_BY_ID_PR
    @P_idInstBancaria INT
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
    WHERE idInstBancaria = @P_idInstBancaria
END
GO
