CREATE PROCEDURE RET_TRANSACCION_BY_CLIENTE_PR
    @P_IdCuentaCliente INT
AS
BEGIN
    SELECT *
    FROM transaccion
    WHERE IdCuentaCliente = @P_IdCuentaCliente;
END
