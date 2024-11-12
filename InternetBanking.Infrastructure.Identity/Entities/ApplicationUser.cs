using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;


namespace InternetBanking.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; } //Cedula
        public bool EstaActivo { get; set; } = true;
        public TipoUsuario TipoUsuario { get; set; }
        public decimal? MontoInicial { get; set; }
        public bool TieneCuentaPrincipal { get; set; } = false;

    }
}
