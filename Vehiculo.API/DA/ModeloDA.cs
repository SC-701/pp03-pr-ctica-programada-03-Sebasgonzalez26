using Abstracciones.Interfaces.DA;
using Dapper;
using Modelos;
using System.Data;

namespace DA
{
    public class ModeloDA : IModeloDA
    {
        private readonly IRepositorioDapper _repositorioDapper;

        public ModeloDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
        }

        public async Task<IEnumerable<Modelo>> ObtenerModelosPorMarca(Guid idMarca)
        {
            using var conexion = _repositorioDapper.ObtenerRepositorio();

            var parametros = new
            {
                IdMarca = idMarca
            };

            var resultado = await conexion.QueryAsync<Modelo>(
                "dbo.ObtenerModelosPorMarca",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }
    }
}