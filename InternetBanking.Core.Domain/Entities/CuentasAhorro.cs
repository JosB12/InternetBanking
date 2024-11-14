

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
        public ProductoFinanciero ProductoFinanciero { get; set; }
        public virtual ICollection<Transacciones> TransaccionesOrigen { get; set; }
        public virtual ICollection<Transacciones> TransaccionesDestino { get; set; }
        public virtual ICollection<Pagos> Pagos { get; set; }
        public virtual ICollection<AvancesEfectivo> AvancesEfectivo { get; set; }
    }
}
