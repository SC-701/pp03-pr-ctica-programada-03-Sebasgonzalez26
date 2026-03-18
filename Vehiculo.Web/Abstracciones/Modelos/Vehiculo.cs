using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    public class VehiculoBase
    {
        [Required(ErrorMessage = "La propiedad placa es requerida")]
        [RegularExpression(@"[A-Za-z]{3}-[0-9]{3}", ErrorMessage = "El formato de la placa es inválido. El formato correcto es 'ABC-123'")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "La propiedad color es requerida")]
        [StringLength(40, ErrorMessage = "El color no puede tener más de 40 caracteres", MinimumLength = 4)]
        public string Color { get; set; }

        [Required(ErrorMessage = "La propiedad año es requerida")]
        [Range(1900, 2099, ErrorMessage = "El formato del año no es valido")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "La propiedad precio es requerida")]

        public Decimal Precio { get; set; }

        [Required(ErrorMessage = "La propiedad correo es requerida")]
        [EmailAddress]

        public string CorreoPropietario { get; set; }


        [Required(ErrorMessage = "La propiedad telefono es requerida")]
        [Phone]

        public string TelefonoPropietario { get; set; }
    }

    public class VehiculoRequest : VehiculoBase
    {
        [Required(ErrorMessage = "La propiedad modelo es requerida")]
        public Guid IdModelo { get; set; }
    }


    public class VehiculoResponse : VehiculoBase
    {
        public Guid Id { get; set; }
        public string? Modelo { get; set; }
        public string? Marca { get; set; }
    }

    public class VehiculoDetalle : VehiculoResponse
    {
        public bool RevisionValida { get; set; }

        public bool RegistroValido { get; set; }
    }

    
}
