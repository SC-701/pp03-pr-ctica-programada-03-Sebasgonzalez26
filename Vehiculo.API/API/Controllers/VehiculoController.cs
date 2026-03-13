using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("practica/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase, IVehiculoController
    {

        private IVehiculoFlujo _vehiculoFlujo;
        private ILogger<VehiculoController> _logger;

        public VehiculoController(IVehiculoFlujo vehiculoFlujo, ILogger<VehiculoController> logger)
        {
            _vehiculoFlujo = vehiculoFlujo;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody]VehiculoRequest Vehiculo)
        {

            var resultado = await _vehiculoFlujo.Agregar(Vehiculo);
            return CreatedAtRoute("ObtenerVehiculoPorId", new { Id = resultado }, null);

        }

        [HttpPut("{Id}")]
        //[Route("/edit/{Id}")]para identificar diferentes url q van a tener las acciones
        public async Task<IActionResult> Editar([FromRoute] Guid Id, [FromBody]VehiculoRequest Vehiculo)
        {

            if (!await VerificarVehiculoExiste(Id))
                return NotFound("El vehiculo no existe");
            
            var resultado = await  _vehiculoFlujo.Editar(Id, Vehiculo);
            return Ok(resultado);
        }

      
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar([FromRoute] Guid Id)
        {
            if (!await VerificarVehiculoExiste(Id))
                return NotFound("El vehiculo no existe");
            var resultado = await  _vehiculoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _vehiculoFlujo.Obtener();

            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [HttpGet("{Id}", Name = "ObtenerVehiculoPorId")]
        public async Task<IActionResult> Obtener([FromRoute]Guid Id)
        {
            var resultado = await _vehiculoFlujo.Obtener(Id);
            return Ok(resultado);
        }
        private async Task<bool> VerificarVehiculoExiste(Guid Id)
        {
            var resultadoValidacion = false;
            var resultadoVehiculoExistente = await _vehiculoFlujo.Obtener(Id);
            if (resultadoVehiculoExistente != null)
                return true;
            return resultadoValidacion;


        }

    }
}
