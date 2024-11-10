using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Enums
{
    public enum TipoTransaccion
    {
        Deposito,
        Retiro,
        Transferencia,
        PagoExpreso,
        PagoTarjetaCredito,
        PagoPrestamo,
        PagoBeneficiario,
        AvanceEfectivo
    }
}
