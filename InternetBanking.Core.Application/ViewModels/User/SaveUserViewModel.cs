using InternetBanking.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;


namespace InternetBanking.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        [Required(ErrorMessage = "Debe colocar el nombre del usuario")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe colocar el apellido del usuario")]
        [DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe colocar una cedula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden")]
        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de usuario")]
        public TipoUsuario TipoUsuario { get; set; } // Puede ser "Admin" o "Client"

        // Solo aparecerá si UserType es "Client"
        public decimal? MontoInicial { get; set; } // Puede ser 0, nullable
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
