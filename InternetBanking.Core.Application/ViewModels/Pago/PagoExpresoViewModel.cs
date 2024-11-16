namespace InternetBanking.Core.Application.ViewModels.Pago
{
    public class PagoExpresoViewModel
    {
        public string NumeroCuenta { get; set; }
        public decimal Monto { get; set; }
        public int IdCuentaPago { get; set; }
        public int CuentaDestino { get; set; }
    }
}