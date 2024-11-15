using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Application.DTOS.General;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Core.Application.Interfaces.Services;

namespace InternetBanking.Core.Application.Services
{
    public class AvancesEfectivoService : IAvancesEfectivoService
    {
        private readonly IAvancesEfectivoRepository _avancesEfectivoRepository;
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;

        public AvancesEfectivoService(
            IAvancesEfectivoRepository avancesEfectivoRepository,
            ITarjetasCreditoRepository tarjetasCreditoRepository,
            ICuentasAhorroRepository cuentasAhorroRepository)
        {
            _avancesEfectivoRepository = avancesEfectivoRepository;
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
            _cuentasAhorroRepository = cuentasAhorroRepository;
        }

        #region Realizar Avance de Efectivo
        public async Task<Response> RealizarAvanceAsync(int tarjetaCreditoId, int cuentaAhorroId, decimal monto)
        {
            try
            {
                // Obtener la tarjeta de crédito y la cuenta de ahorro
                var tarjeta = await _tarjetasCreditoRepository.GetByIdAsync(tarjetaCreditoId);
                var cuenta = await _cuentasAhorroRepository.GetByIdAsync(cuentaAhorroId);

                if (tarjeta == null || cuenta == null)
                {
                    return new Response { Success = false, Message = "Tarjeta de crédito o cuenta de ahorro no encontrados." };
                }

                // Validar que el monto no supere el límite de crédito disponible
                if (monto > (tarjeta.LimiteCredito - tarjeta.DeudaActual))
                {
                    return new Response { Success = false, Message = "El monto excede el límite de crédito disponible." };
                }

                // Calcular el interés (6.25%) y actualizar los saldos
                var interes = monto * 0.0625m;
                tarjeta.DeudaActual += monto + interes;
                cuenta.Balance += monto;

                // Guardar los cambios en la base de datos
                await _tarjetasCreditoRepository.UpdateAsync(tarjeta);
                await _cuentasAhorroRepository.UpdateGeneralAsync(cuenta);

                // Registrar el avance de efectivo (opcional)
                var avance = new AvancesEfectivo
                {
                    IdTarjetaCredito = tarjetaCreditoId,
                    IdCuentaDestino = cuentaAhorroId,
                    Monto = monto,
                    FechaAvance = DateTime.UtcNow
                };
                await _avancesEfectivoRepository.AddAsync(avance);

                return new Response { Success = true, Message = "Avance de efectivo realizado exitosamente." };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = "Error al procesar el avance: " + ex.Message };
            }
        }
        #endregion

        #region Obtener todos los avances de efectivo
        public async Task<List<AvanceEfectivoViewModel>> GetAllAvancesAsync()
        {
            try
            {
                // Obtener todos los avances de efectivo desde el repositorio
                var avances = await _avancesEfectivoRepository.GetAllAsync();

                // Mapear los avances a un ViewModel o DTO
                var avancesViewModel = avances.Select(a => new AvanceEfectivoViewModel
                {
                    TarjetaCreditoId = a.IdTarjetaCredito,
                    CuentaAhorroId = a.IdCuentaDestino,
                    Monto = a.Monto,
                    // Otros campos que quieras mapear
                }).ToList();

                return avancesViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los avances de efectivo: " + ex.Message);
            }
        }
        #endregion
    }
}
