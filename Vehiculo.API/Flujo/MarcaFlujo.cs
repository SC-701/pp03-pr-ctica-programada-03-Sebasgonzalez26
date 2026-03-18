using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Modelos;

namespace Flujo
{
    public class MarcaFlujo : IMarcaFlujo
    {
        private readonly IMarcaDA _marcaDA;

        public MarcaFlujo(IMarcaDA marcaDA)
        {
            _marcaDA = marcaDA;
        }

        public async Task<IEnumerable<Marca>> ObtenerMarcas()
        {
            return await _marcaDA.ObtenerMarcas();
        }
    }
}