﻿CREATE PROCEDURE RET_CUENTACOMERCIO_BY_TELEFONO_PR
    @P_telefono INT
AS
BEGIN
    SELECT 
        idCuenta,
        nombreUsuario,
        contrasena,
        cedulaJuridica,
        telefono,
        correoElectronico,
        direccion
    FROM cuentaComercio
    WHERE telefono = @P_telefono
END
GO
