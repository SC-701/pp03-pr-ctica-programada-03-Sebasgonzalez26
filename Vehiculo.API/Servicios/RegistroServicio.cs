using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Registro;
using System.Net;
using System.Text.Json;

namespace Servicios
{
    public class RegistroServicio : IRegistroServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        public RegistroServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Propietario> Obtener(string placa)
        {
            var endPoint = _configuracion.ObtenerMetodo("ApiEndPointsRegistro", "ObtenerRegistros");
            var servicioRegistro = _httpClientFactory.CreateClient("ServicioRegistro");

            var url = string.Format(endPoint, placa);

            var respuesta = await servicioRegistro.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();

            respuesta.EnsureSuccessStatusCode();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var lista = JsonSerializer.Deserialize<List<Propietario>>(resultado, opciones);

            return lista?.FirstOrDefault();
        }
    }
}