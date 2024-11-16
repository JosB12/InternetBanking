namespace InternetBanking.Core.Application.ViewModels.Pago
{

    public class PagoViewModel
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Monto {get; set; }
        public int IdCuentaPago { get; set; }
        public int IdTarjetaCredito { get; set; }
        public int CuentaDestino { get; set; }
    }
}
