using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PruebaTecnica.Core.DataProviders.Entity.Seguridad
{
    [Table("Usuario")]
    public class Usuario : IdentityUser
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public int Identificacion { get; set; }

        [Required]
        public int TipoIdentificacionId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }

        [MinLength(8, ErrorMessage = "La clave debe contener mas 8 caracteres")]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 8)]
        [RegularExpression("^([\\da-z_\\.-]+)@([\\da-z\\.-]+)\\.([a-z\\.]{2,6})$", ErrorMessage = "Correo no cumple la recomendacion")]
        public string Email { get; set; }

    }
}
