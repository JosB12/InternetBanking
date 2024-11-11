namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiarios
    {

        public int Id { get; set; }  
        public string? IdUsuario { get; set; }
        public int IdCuentaBeneficiario { get; set; }  
        public CuentasAhorro CuentaBeneficiario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroCuenta { get; set; }
    }
}
