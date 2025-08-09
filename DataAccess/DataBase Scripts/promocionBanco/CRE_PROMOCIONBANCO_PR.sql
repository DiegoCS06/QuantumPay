CREATE PROCEDURE CRE_PROMOCIONBANCO_PR
    @P_Nombre NVARCHAR(100),
    @P_Descripcion NVARCHAR(200),
    @P_Descuento DECIMAL(5,4),
    @P_FechaInicio DATE,
    @P_FechaFin DATE
AS
BEGIN
    INSERT INTO promocionBanco (Nombre, Descripcion, Descuento, FechaInicio, FechaFin)
    VALUES (@P_Nombre, @P_Descripcion, @P_Descuento, @P_FechaInicio, @P_FechaFin);
END