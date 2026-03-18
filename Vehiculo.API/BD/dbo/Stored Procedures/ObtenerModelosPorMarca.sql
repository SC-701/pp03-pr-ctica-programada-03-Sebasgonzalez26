
CREATE PROCEDURE dbo.ObtenerModelosPorMarca
    @IdMarca UNIQUEIDENTIFIER
AS
BEGIN
    SELECT Id, IdMarca, Nombre
    FROM Modelos
    WHERE IdMarca = @IdMarca
END