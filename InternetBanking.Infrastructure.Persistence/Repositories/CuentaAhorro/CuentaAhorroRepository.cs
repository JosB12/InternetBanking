using InternetBanking.Core.Application.Interfaces.Repositories.CuentaAhorro;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories.CuentaAhorro
{
    public class CuentaAhorroRepository : GenericRepository<CuentasAhorro>, ICuentaAhorroRepository
    {
        private readonly ApplicationContext _dbContext;

        public CuentaAhorroRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(CuentasAhorro cuentaOrigen)
        {
            // Verifica si la cuenta existe en la base de datos
            var cuentaExistente = await _dbContext.CuentasAhorro
                .FirstOrDefaultAsync(c => c.Id == cuentaOrigen.Id);

            if (cuentaExistente == null)
            {
                throw new KeyNotFoundException("La cuenta de ahorro no existe.");
            }

            // Aquí se actualizan las propiedades que pueden cambiar, por ejemplo:
            cuentaExistente.Balance = cuentaOrigen.Balance; // Actualiza el saldo

            // Si tienes otras propiedades que quieres actualizar, agrégalas aquí
            // Ejemplo:
            // cuentaExistente.Propiedad = cuentaOrigen.Propiedad;

            // Marcar la entidad como modificada
            _dbContext.CuentasAhorro.Update(cuentaExistente);

            // Guardar los cambios en la base de datos de manera asincrónica
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CuentasAhorro>> ObtenerCuentasPorUsuarioAsync(string usuarioId)
        {
            // Obtener las cuentas de ahorro del usuario, incluyendo el producto financiero asociado
            var cuentasUsuario = await _dbContext.CuentasAhorro
                .Where(c => c.ProductoFinanciero.IdUsuario == usuarioId) // Filtra las cuentas por el usuario
                .Include(c => c.ProductoFinanciero) // Incluir el producto financiero asociado a cada cuenta
                .ToListAsync(); // Ejecuta la consulta y materializa los datos

            if (cuentasUsuario == null || !cuentasUsuario.Any())
            {
                // Si no hay cuentas encontradas para el usuario, devuelve una lista vacía
                return new List<CuentasAhorro>();
            }

            return cuentasUsuario;
        }






    }
}
