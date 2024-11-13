using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string Usuario { get; set; }
        public bool EstaActivo { get; set; }
        public string TipoUsuario { get; set; } 

        public List<MostrarProductoViewModel> Productos { get; set; }

        public UserViewModel()
        {
            Productos = new List<MostrarProductoViewModel>();
        }
    }
}
