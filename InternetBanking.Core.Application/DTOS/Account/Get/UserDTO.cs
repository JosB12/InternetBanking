namespace InternetBanking.Core.Application.DTOS.Account.Get
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string TipoUsuario { get; set; }
        public bool EstaActivo { get; set; }
    }
}
