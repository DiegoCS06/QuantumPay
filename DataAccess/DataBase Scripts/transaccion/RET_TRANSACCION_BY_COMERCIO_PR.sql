CREATE PROCEDURE RET_TRANSACCION_BY_COMERCIO_PR
    @P_IdCuentaComercio INT
AS
BEGIN
    SELECT *
    FROM transaccion
    WHERE IdCuentaComercio = @P_IdCuentaComercio;
END
