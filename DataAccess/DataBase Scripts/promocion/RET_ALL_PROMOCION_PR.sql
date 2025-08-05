CREATE PROCEDURE RET_ALL_PROMOCION_PR
AS
BEGIN
    SELECT 
        Id AS id,
        Nombre AS nombre,
        Descripcion AS descripcion,
        Descuento AS descuento,
        FechaInicio AS fechaInicio,
        FechaFin AS fechaFin
    FROM Promocion;
END