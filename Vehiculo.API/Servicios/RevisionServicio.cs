using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Revision;
using System.Net;
using System.Text.Json;

namespace Servicios
{
    public class RevisionServicio : IRevisionServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        public RevisionServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Revision?> Obtener(string placa)
        {
            var endPoint = _configuracion.ObtenerMetodo("ApiEndPointsRevision", "ObtenerRevision");
            var servicioRegistro = _httpClientFactory.CreateClient("ServicioRevision");

            var respueta = await servicioRegistro.GetAsync(string.Format(endPoint, placa));

            if (respueta.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var resultado = await respueta.Content.ReadAsStringAsync();

            respueta.EnsureSuccessStatusCode();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadDeserializado = JsonSerializer.Deserialize<List<Revision>>(resultado, opciones);

            return resultadDeserializado?.FirstOrDefault();
        }
    }
}