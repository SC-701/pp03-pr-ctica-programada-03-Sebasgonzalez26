using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.DA
{
    public interface IVehiculoDA
    {

                Task<IEnumerable<VehiculoResponse>> Obtener();
                Task<VehiculoDetalle> Obtener(Guid Id);
                Task<Guid> Agregar(VehiculoRequest Vehiculo);

                Task<Guid>  Editar(Guid Id, VehiculoRequest Vehiculo);
                Task<Guid> Eliminar(Guid Id);
    }

}
