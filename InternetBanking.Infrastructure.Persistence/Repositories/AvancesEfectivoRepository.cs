using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class AvancesEfectivoRepository : GenericRepository<AvancesEfectivo>, IAvancesEfectivoRepository
    {
        private readonly ApplicationContext _dbContext;

        public AvancesEfectivoRepository(ApplicationContext dbContext)
            : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        // Obtener tarjeta de crédito por ID
        public async Task<TarjetasCredito> GetTarjetaCreditoByIdAsync(string tarjetaCreditoId)
        {
            return await _dbContext.TarjetasCredito
                .Include(tc => tc.ProductoFinanciero)
                .Where(tc => tc.ProductoFinanciero.NumeroProducto == tarjetaCreditoId)
                .FirstOrDefaultAsync();
        }

      

        // Obtener cuenta de ahorro por ID
        public async Task<CuentasAhorro> GetCuentaAhorroByIdAsync(string cuentaAhorroId)
        {
            return await _dbContext.CuentasAhorro
                .Include(ca => ca.ProductoFinanciero)
                .Where(ca => ca.ProductoFinanciero.NumeroProducto == cuentaAhorroId)
                .FirstOrDefaultAsync();
        }

        // Actualizar el balance de la cuenta de ahorro después del avance
        public async Task<bool> UpdateCuentaAhorroBalanceAsync(string cuentaAhorroId, decimal nuevoBalance)
        {
            var cuentaAhorro = await GetCuentaAhorroByIdAsync(cuentaAhorroId);
            if (cuentaAhorro != null)
            {
                cuentaAhorro.Balance = nuevoBalance;
                _dbContext.CuentasAhorro.Update(cuentaAhorro);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Actualizar deuda de la tarjeta de crédito con el monto del avance y el interés
        public async Task<bool> UpdateTarjetaCreditoDeudaAsync(string tarjetaCreditoId, decimal montoAvance)
        {
            var tarjetaCredito = await GetTarjetaCreditoByIdAsync(tarjetaCreditoId);
            if (tarjetaCredito != null)
            {
                decimal deudaNueva = tarjetaCredito.DeudaActual + montoAvance + (montoAvance * 0.0625m);
                tarjetaCredito.DeudaActual = deudaNueva;
                _dbContext.TarjetasCredito.Update(tarjetaCredito);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
