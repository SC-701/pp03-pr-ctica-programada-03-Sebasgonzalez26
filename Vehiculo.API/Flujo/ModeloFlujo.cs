using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Modelos;

namespace Flujo
{
    public class ModeloFlujo : IModeloFlujo
    {
        private readonly IModeloDA _modeloDA;

        public ModeloFlujo(IModeloDA modeloDA)
        {
            _modeloDA = modeloDA;
        }

        public async Task<IEnumerable<Modelo>> ObtenerModelosPorMarca(Guid idMarca)
        {
            return await _modeloDA.ObtenerModelosPorMarca(idMarca);
        }
    }
}