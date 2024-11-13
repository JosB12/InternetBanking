using InternetBanking.Core.Application.Enums;

namespace InternetBanking.Core.Application.ViewModels.User
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public decimal? MontoAdicional { get; set; } // Solo visible si el usuario es Cliente
        public TipoUsuario TipoUsuario { get; set; }
    }
}
