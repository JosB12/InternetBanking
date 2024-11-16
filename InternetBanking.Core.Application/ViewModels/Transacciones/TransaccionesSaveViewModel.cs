using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.ViewModels.Transacciones
{
public class TransaccionesSaveViewModel
{
    public int Id { get; set; }
    public string IdUsuario { get; set; }
    public TipoTransaccion TipoTransaccion { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public int? IdCuentaOrigen { get; set; }
    public int? IdCuentaDestino { get; set; }
    public int? IdProductoFinanciero { get; set; }
}
}
