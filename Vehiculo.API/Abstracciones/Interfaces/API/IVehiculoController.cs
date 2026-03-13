

using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IVehiculoController
    {

        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(Guid Id);
        Task<IActionResult> Agregar(VehiculoRequest Vehiculo);

        Task<IActionResult> Editar(Guid Id, VehiculoRequest Vehiculo);
        Task<IActionResult> Eliminar(Guid Id);

    }
}
