﻿CREATE PROCEDURE RET_ALL_INSTITUCIONBANCARIA_PR
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
        correoElectronico
    FROM institucionBancaria
END
GO
