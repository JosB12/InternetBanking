using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels
{
    public class DashboardViewModel
    {
        public int TransaccionesTotales { get; set; }
        public int TransaccionesHoy { get; set; }
        public int PagosHoy { get; set; }
        public int TotalPagos { get; set; }
        public int ClientesActivos { get; set; }
        public int ClientesInactivos { get; set; }
        public int ProductosAsignados { get; set; }

         public List<ProductosFinancierosViewModel>? Productos { get; set; }
    }
}
