CREATE PROCEDURE CRE_PROMOCION_PR
    @P_Nombre NVARCHAR(100),
    @P_Descripcion NVARCHAR(500),
    @P_Descuento DECIMAL(5,4),
    @P_FechaInicio DATETIME,
    @P_FechaFin DATETIME
AS
BEGIN
    INSERT INTO promocion (Nombre, Descripcion, Descuento, FechaInicio, FechaFin)
    VALUES (@P_Nombre, @P_Descripcion, @P_Descuento, @P_FechaInicio, @P_FechaFin);
END