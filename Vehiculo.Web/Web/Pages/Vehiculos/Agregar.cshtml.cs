using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Pages.Vehiculos
{
    public class AgregarModel : PageModel
    {


        private readonly IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }


        [BindProperty]
        public VehiculoRequest Vehiculo { get; set; }
        [BindProperty]
        public List<SelectListItem> marcas { get; set; }
        [BindProperty]
        public List<SelectListItem> modelos { get; set; }
        [BindProperty]
        public Guid marcaseleccionada { get; set; }

        

        public async Task<ActionResult> OnGet()
        {
            await ObtenerMarcas();
            return Page();

        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await ObtenerMarcas();

                if (marcaseleccionada != Guid.Empty)
                {
                    var listaModelos = await ObtenerModelos(marcaseleccionada);
                    modelos = listaModelos.Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nombre
                    }).ToList();
                }

                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("APIEndPoints", "AgregarVehiculo");

            var cliente = new HttpClient();
            var respuesta = await cliente.PostAsJsonAsync(endpoint, Vehiculo);

            var contenido = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                await ObtenerMarcas();

                if (marcaseleccionada != Guid.Empty)
                {
                    var listaModelos = await ObtenerModelos(marcaseleccionada);
                    modelos = listaModelos.Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nombre
                    }).ToList();
                }

                ModelState.AddModelError(string.Empty, $"Error API: {contenido}");
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("APIEndPoints", "ObtenerMarcas");


            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var resultadodeserializado = JsonSerializer.Deserialize<List<Marca>>(resultado, opciones);

            marcas = resultadodeserializado.Select(m => 
            new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre,

            }
            ).ToList();
        }



        private async Task<List<Modelo>> ObtenerModelos(Guid marcaID)
        {
            string endpoint = _configuracion.ObtenerMetodo("APIEndPoints", "ObtenerModelos");


            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, marcaID));

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

           return JsonSerializer.Deserialize<List<Modelo>>(resultado, opciones);

         
        }


        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaID)
        {
            var modelos = await ObtenerModelos(marcaID);
            return new JsonResult(modelos);

        }



    }
}
