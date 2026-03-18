using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("practica/[controller]")]
    [ApiController]
    public class ModeloController : ControllerBase, IModeloController
    {
        private readonly IModeloFlujo _modeloFlujo;
        private readonly ILogger<ModeloController> _logger;

        public ModeloController(IModeloFlujo modeloFlujo, ILogger<ModeloController> logger)
        {
            _modeloFlujo = modeloFlujo;
            _logger = logger;
        }

        [HttpGet("{idMarca}")]
        public async Task<IActionResult> ObtenerPorMarca([FromRoute] Guid idMarca)
        {
            var resultado = await _modeloFlujo.ObtenerModelosPorMarca(idMarca);

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}