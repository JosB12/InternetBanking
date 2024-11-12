using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class CuentasAhorroRepository : GenericRepository<CuentasAhorro>, ICuentasAhorroRepository
    {
        private readonly ApplicationContext _dbContext;

        public CuentasAhorroRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<CuentasAhorro> GetAccountByIdAndNumeroCuentaAsync(int idCuenta, string numeroCuenta)
        {
            // Verificar que los parámetros no sean nulos o vacíos
            if (idCuenta <= 0 || string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El ID de cuenta o el número de cuenta no son válidos.");
            }

            // Buscar la cuenta en la base de datos, incluyendo información de Producto y Usuario si están relacionadas
            var cuenta = await _dbContext.CuentasAhorro
                .Include(c => c.ProductoFinanciero) // Incluye la entidad Producto relacionada
                .ThenInclude(p => p.IdUsuario) // Luego incluye el Usuario relacionado con el Producto (si es aplicable)
                .FirstOrDefaultAsync(c => c.Id == idCuenta && c.NumeroCuenta.Trim() == numeroCuenta.Trim());

            // Si no se encuentra la cuenta, lanzar una excepción
            if (cuenta == null)
            {
                throw new KeyNotFoundException("No se encontró una cuenta con el ID y número de cuenta proporcionados.");
            }

            return cuenta;
        }


        public async Task<CuentasAhorro> GetAccountByNumeroCuentaAsync(string numeroCuenta)
        {
            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El número de cuenta no puede ser nulo o vacío.");
            }

            var trimmedCuenta = numeroCuenta.Trim();
            Console.WriteLine($"Buscando cuenta con el número: {trimmedCuenta}");

            // Verificar que _dbContext no sea nulo
            if (_dbContext == null)
            {
                throw new InvalidOperationException("DbContext no está inicializado.");
            }

            // Realizar la consulta sin tratar NumeroCuenta como un tipo numérico
            var cuenta = await _dbContext.CuentasAhorro
                .FirstOrDefaultAsync(c => c.NumeroCuenta == trimmedCuenta);

            if (cuenta == null)
            {
                throw new KeyNotFoundException($"No se encontró una cuenta con el número {numeroCuenta}");
            }

            return cuenta;
        }








    }
}
