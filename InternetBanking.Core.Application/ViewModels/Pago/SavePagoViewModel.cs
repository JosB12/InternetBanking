namespace InternetBanking.Core.Application.ViewModels.Pago
{


    public class SavePagoViewModel
    {
     
        public decimal Monto { get; set; }
        public int IdCuentaPago { get; set; }
        public int IdTarjetaCredito { get; set; }
    }
}
