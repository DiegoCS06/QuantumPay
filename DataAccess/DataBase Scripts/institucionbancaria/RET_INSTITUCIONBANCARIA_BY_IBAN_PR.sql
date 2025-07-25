CREATE PROCEDURE RET_INSTITUCIONBANCARIA_BY_IBAN_PR
    @P_codigoIBAN  NVARCHAR(30)
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
    WHERE codigoIBAN = @P_codigoIBAN
END
GO
