using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public VehiculoRequest VehiculoRequest { get; set; }
        public VehiculoResponse? VehiculoResponse { get; set; }
        [BindProperty]
        public List<SelectListItem> marcas { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> modelos { get; set; } = new();

        [BindProperty]
        public Guid marcaseleccionada { get; set; }

        [BindProperty]
        public Guid modeloseleccionada { get; set; }

        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("APIEndPoints", "ObtenerVehiculo");

            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);
            var contenido = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception($"Error API: {respuesta.StatusCode} - {contenido}");
            }

            await ObtenerMarcas();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            VehiculoResponse = JsonSerializer.Deserialize<VehiculoResponse>(contenido, opciones);

            if (VehiculoResponse != null)
            {
                Id = VehiculoResponse.Id;

                var marcaSeleccionada = marcas.FirstOrDefault(m => m.Text == VehiculoResponse.Marca);
                if (marcaSeleccionada != null)
                {
                    marcaseleccionada = Guid.Parse(marcaSeleccionada.Value);
                }

                modelos = (await ObtenerModelos(marcaseleccionada)).Select(m =>
                    new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nombre,
                        Selected = m.Nombre == VehiculoResponse.Modelo
                    }).ToList();

                var modeloSeleccionado = modelos.FirstOrDefault(m => m.Text == VehiculoResponse.Modelo);
                if (modeloSeleccionado != null)
                {
                    modeloseleccionada = Guid.Parse(modeloSeleccionado.Value);
                }

                VehiculoRequest = new VehiculoRequest
                {
                    IdModelo = modeloseleccionada,
                    Placa = VehiculoResponse.Placa,
                    Precio = VehiculoResponse.Precio,
                    Anio = VehiculoResponse.Anio,
                    Color = VehiculoResponse.Color,
                    CorreoPropietario = VehiculoResponse.CorreoPropietario,
                    TelefonoPropietario = VehiculoResponse.TelefonoPropietario
                };
            }

            return Page();
        }

        public async Task<ActionResult> OnPost(Guid id)
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

            string endpoint = _configuracion.ObtenerMetodo("APIEndPoints", "EditarVehiculo");

            var cliente = new HttpClient();
            var respuesta = await cliente.PutAsJsonAsync(string.Format(endpoint, id), VehiculoRequest);

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

            marcas = resultadodeserializado.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre,
            }).ToList();
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