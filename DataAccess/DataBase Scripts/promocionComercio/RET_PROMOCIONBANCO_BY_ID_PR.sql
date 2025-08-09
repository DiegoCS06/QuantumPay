CREATE PROCEDURE RET_PROMOCIONCOMERCIO_BY_ID_PR
    @P_Id INT
AS
BEGIN
    SELECT 
        Id AS id,
        Nombre AS nombre,
        Descripcion AS descripcion,
        Descuento AS descuento,
        FechaInicio AS fechaInicio,
        FechaFin AS fechaFin
    FROM promocionComercio
    WHERE Id = @P_Id;
END