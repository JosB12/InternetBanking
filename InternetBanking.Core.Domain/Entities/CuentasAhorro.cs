

namespace InternetBanking.Core.Domain.Entities
{
    public class CuentasAhorro
    {
        public int Id { get; set; }  
        public string IdentificadorUnico { get; set; }  
        public string NumeroCuenta { get; set; } 
        public decimal Balance { get; set; }
        public bool EsPrincipal { get; set; }

        public int IdProductoFinanciero { get; set; }  
        public ProductosFinancieros ProductoFinanciero { get; set; }
    }
}
