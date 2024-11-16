using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Pago;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class PagoService : GenericService<SavePagoViewModel, PagoViewModel, Pagos>, IPagoService
    {
        private readonly IGenericRepository<Pagos> _pagosRepository;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;
        private readonly IMapper _mapper;

        public PagoService(IGenericRepository<Pagos> pagosRepository, ICuentasAhorroRepository cuentasAhorroRepository, ITarjetasCreditoRepository tarjetasCreditoRepository, IMapper mapper)
            : base(pagosRepository, mapper)
        {
            _pagosRepository = pagosRepository;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
            _mapper = mapper;
        }

        public async Task<string> RealizarPagoExpreso(SavePagoExpresoViewModel model)
        {
            var cuentaDestino = await _cuentasAhorroRepository.GetCuentaByNumeroCuentaAsync(model.NumeroCuenta);
            if (cuentaDestino == null)
            {
                return "La cuenta destino no existe.";
            }

            var cuentaOrigen = await _cuentasAhorroRepository.GetByIdAsync(model.IdCuentaPago);
            if (cuentaOrigen.Balance < model.Monto)
            {
                return "Saldo insuficiente en la cuenta origen.";
            }

            // Realizar la transacción
            cuentaOrigen.Balance -= model.Monto;
            cuentaDestino.Balance += model.Monto;

            await _cuentasAhorroRepository.UpdateAsync(cuentaOrigen, cuentaOrigen.Id);
            await _cuentasAhorroRepository.UpdateAsync(cuentaDestino, cuentaDestino.Id);

            // Registrar el pago
            var pago = new Pagos
            {
                IdUsuario = cuentaOrigen.ProductoFinanciero.IdUsuario,
                TipoPago = TipoPago.Expreso,
                Monto = model.Monto,
                Fecha = DateTime.Now,
                IdCuentaPago = cuentaOrigen.Id,
                IdCuentaDestino = cuentaDestino.Id
            };

            await _pagosRepository.AddAsync(pago);

            return "Pago realizado con éxito.";
        }

        public async Task<string> RealizarPagoTarjetaCredito(SavePagoViewModel model)
        {
            var tarjeta = await _tarjetasCreditoRepository.GetByIdAsync(model.IdTarjetaCredito);
            if (tarjeta == null)
            {
                return "La tarjeta de crédito no existe.";
            }

            var cuenta = await _cuentasAhorroRepository.GetByIdAsync(model.IdCuentaPago);
            if (cuenta == null)
            {
                return "La cuenta de pago no existe.";
            }

            if (cuenta.Balance < model.Monto)
            {
                return "Saldo insuficiente en la cuenta de pago.";
            }

            var montoAPagar = Math.Min(model.Monto, tarjeta.LimiteCredito);

            // Realizar el pago
            cuenta.Balance -= montoAPagar;
            tarjeta.LimiteCredito -= montoAPagar;

            await _cuentasAhorroRepository.UpdateAsync(cuenta, cuenta.Id);
            await _tarjetasCreditoRepository.UpdateAsync(tarjeta, tarjeta.Id);

            // Registrar el pago
            var pago = new Pagos
            {
                IdUsuario = cuenta.ProductoFinanciero.IdUsuario, // Ensure the user ID is set
                TipoPago = TipoPago.TarjetaCredito,
                Monto = montoAPagar,
                Fecha = DateTime.Now,
                IdCuentaPago = cuenta.Id,
                IdProductoFinanciero = tarjeta.Id
            };

            await _pagosRepository.AddAsync(pago);

            return "Pago realizado con éxito.";
        }

        public async Task<IEnumerable<CuentasAhorro>> GetCuentasByUserIdAsync(string userId)
        {
            return await _cuentasAhorroRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<TarjetasCredito>> GetTarjetasByUserIdAsync(string userId)
        {
            return await _tarjetasCreditoRepository.GetByUserIdAsync(userId);
        }

        public async Task<CuentasAhorro> GetCuentaByIdentificadorUnicoAsync(string identificadorUnico)
        {
            return await _cuentasAhorroRepository.GetByIdentificadorUnicoAsync(identificadorUnico);
        }

        public async Task<CuentasAhorro> GetCuentaByNumeroCuentaAsync(string numeroCuenta)
        {
            return await _cuentasAhorroRepository.GetCuentaByNumeroCuentaAsync(numeroCuenta);
        }

    }
}
