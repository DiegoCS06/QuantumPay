CREATE PROCEDURE UPD_PROMOCION_PR
    @P_Id INT,
    @P_Nombre NVARCHAR(100),
    @P_Descripcion NVARCHAR(500),
    @P_Descuento DECIMAL(5,4),
    @P_FechaInicio DATETIME,
    @P_FechaFin DATETIME
AS
BEGIN
    UPDATE promocion
    SET Nombre = @P_Nombre,
        Descripcion = @P_Descripcion,
        Descuento = @P_Descuento,
        FechaInicio = @P_FechaInicio,
        FechaFin = @P_FechaFin
    WHERE Id = @P_Id;
END