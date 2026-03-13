using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Revision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class RevisonReglas : IRevisionReglas
    {
        private readonly IRevisionServicio _revisionServicio;
        private readonly IConfiguracion _configuracion;

        public RevisonReglas(IRevisionServicio revisionServicio, IConfiguracion configuracion)
        {
            _revisionServicio = revisionServicio;
            _configuracion = configuracion;
        }

        public async Task<bool> RevisionEsValida(string placa)
        {
            var resultadoRevision = await _revisionServicio.Obtener(placa);
            if (ValidarEstado(resultadoRevision) && ValidarPeriodo(resultadoRevision.Periodo))
                return true;
            return false;




        }

        private bool ValidarEstado(Revision resultadoRevision)
        {
           string estadoRevision = _configuracion.ObtenerValor("EstadoRevisionSatisfactorio");
            return resultadoRevision.Resultado == estadoRevision;
        }


        private static string ObtenerPrecioActual()
        {
            return $"{DateTime.Now.Month}-{DateTime.Now.Year}";
        }

        private static bool ValidarPeriodo(string periodo)
        {
            var periodoActual = ObtenerPrecioActual();
            return periodo == periodoActual;
        }
    }
}
