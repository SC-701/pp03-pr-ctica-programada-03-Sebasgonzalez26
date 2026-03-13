using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IRegistroReglas
    {
        Task<bool> VehiculoEstadoRegistro(string placa, string email);
    }
}
