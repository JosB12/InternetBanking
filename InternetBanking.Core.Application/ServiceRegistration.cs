using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Service
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IProductosFinancierosService, ProductosFinancierosService>();
<<<<<<< HEAD
             services.AddTransient<ITransaccionesService, TransaccionesService>();
             services.AddTransient<IPagoService, PagoService>();
=======
            services.AddTransient<IBeneficiarioService, BeneficiarioService>();
>>>>>>> origin/client-beneficiaries-prueba



            #endregion
        }
    }
}
