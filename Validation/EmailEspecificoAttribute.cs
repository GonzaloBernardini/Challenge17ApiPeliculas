using Challenge17ApiPeliculas.IdentityAuth;
using Challenge17ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Validation
{
    public class EmailEspecificoAttribute :  ValidationAttribute
    {
        public EmailEspecificoAttribute(string email)
        {
            Email = email;
        }
        public string Email { get; }
        public string GetErrorMessage() => $"Debe elegir otro tipo de proveedor de email que no sea {Email}";
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            var user = (RegisterModel)validationContext.ObjectInstance;
            var emailIncorrecto = Email;

            if (user.Email.Contains(emailIncorrecto))
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
