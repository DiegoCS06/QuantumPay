﻿CREATE PROCEDURE DEL_CUENTACOMERCIO_PR
    @P_idCuenta INT
AS
BEGIN
    DELETE FROM cuentaComercio
    WHERE idCuenta = @P_idCuenta
END
GO
