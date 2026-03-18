using Abstracciones.Interfaces.DA;
using Dapper;
using Modelos;
using System.Data;

namespace DA
{
    public class MarcaDA : IMarcaDA
    {
        private readonly IRepositorioDapper _repositorioDapper;

        public MarcaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
        }

        public async Task<IEnumerable<Marca>> ObtenerMarcas()
        {
            using var conexion = _repositorioDapper.ObtenerRepositorio();

            var resultado = await conexion.QueryAsync<Marca>(
                "dbo.ObtenerMarcas",
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }
    }
}