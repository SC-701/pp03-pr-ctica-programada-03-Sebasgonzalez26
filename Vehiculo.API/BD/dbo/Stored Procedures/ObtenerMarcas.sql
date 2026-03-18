CREATE PROCEDURE dbo.ObtenerMarcas
AS
BEGIN
    SELECT Id, Nombre
    FROM Marcas
END