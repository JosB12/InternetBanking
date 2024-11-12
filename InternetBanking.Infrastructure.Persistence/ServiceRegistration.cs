using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Application.Interfaces.Repositories.Pago;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using InternetBanking.Infrastructure.Persistence.Repositories.Transaccion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), m =>
                m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPagosRepository, PagoRepository>();
            services.AddTransient<IProductosFinancierosRepository, ProductosFinancierosRepository>();
            services.AddTransient<ITransaccionesRepository, TransaccionesRepository>();


            #endregion
        }
    }
}
