CREATE PROCEDURE UPD_PROMOCIONBANCO_PR
    @P_Id INT,
    @P_Nombre NVARCHAR(100),
    @P_Descripcion NVARCHAR(200),
    @P_Descuento DECIMAL(5,4),
    @P_FechaInicio DATE,
    @P_FechaFin DATE
AS
BEGIN
    UPDATE promocionBanco
    SET Nombre = @P_Nombre,
        Descripcion = @P_Descripcion,
        Descuento = @P_Descuento,
        FechaInicio = @P_FechaInicio,
        FechaFin = @P_FechaFin
    WHERE Id = @P_Id;
END