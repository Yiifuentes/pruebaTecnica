using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Web.Dto
{
    public class UsuarioDto
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public int Identificacion { get; set; }

        [Required]
        public int TipoIdentificacionId { get; set; }
 
        [MinLength(8, ErrorMessage = "La clave debe contener mas 8 caracteres")]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 8)]
        [RegularExpression("^([\\da-z_\\.-]+)@([\\da-z\\.-]+)\\.([a-z\\.]{2,6})$", ErrorMessage = "Correo no cumple la recomendacion")]
        public string Email { get; set; }
    }
}
