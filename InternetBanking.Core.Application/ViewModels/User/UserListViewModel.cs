namespace InternetBanking.Core.Application.ViewModels.User
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; } 
        public string TipoUsuario { get; set; }
        public bool EstaActivo { get; set; }
    }
}
