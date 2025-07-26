CREATE PROCEDURE RET_INSTITUCIONBANCARIA_BY_TELEFONO_PR
    @P_telefono INT
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
    WHERE telefono = @P_telefono
END
GO
