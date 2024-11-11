using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.DTOS.Account.Register
{
    public class RegisterRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Telefono { get; set; }
        public decimal? MontoInicial { get; set; }
        public string TipoUsuario { get; set; }
        public string Cedula { get; set; }

}
}
