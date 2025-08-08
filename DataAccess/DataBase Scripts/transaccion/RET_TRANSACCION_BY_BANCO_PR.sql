CREATE PROCEDURE RET_TRANSACCION_BY_BANCO_PR
    @P_IdCuentaBancaria INT
AS
BEGIN
    SELECT *
    FROM transaccion
    WHERE IdCuentaBancaria = @P_IdCuentaBancaria;
END
