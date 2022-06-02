using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        public string UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
    }
}
