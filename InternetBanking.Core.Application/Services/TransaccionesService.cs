using AutoMapper;
using InternetBanking.Core.Application.DTOS.Account.Authentication;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories.CuentaAhorro;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Core.Application.Interfaces.Services.Account;
using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.Interfaces.Services.Transaccion;
using InternetBanking.Core.Application.Interfaces.Services.User;
using InternetBanking.Core.Application.ViewModels.Transferencia;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class TransaccionesService : IGenericService<TransaccionesViewModel, TransaccionesViewModel, Transacciones>, ITransaccionesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IAccountService _accountService;
        private readonly ICuentaAhorroRepository _cuentasAhorroRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITransaccionesRepository _transaccionesRepository;

        public TransaccionesService(
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IAccountService accountService,
            ICuentaAhorroRepository cuentasAhorroRepository,
            IMapper mapper,
            ITransaccionesRepository transaccionesRepository)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _mapper = mapper;
            _transaccionesRepository = transaccionesRepository;

            // Asignación del usuario autenticado desde la sesión actual
            userViewModel = _httpContextAccessor.HttpContext?.Session?.Get<AuthenticationResponse>("user");
        }

        public Task<TransaccionesViewModel> Add(TransaccionesViewModel vm)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransaccionesViewModel>> GetAllViewModel()
        {
            throw new NotImplementedException();
        }

        public Task<TransaccionesViewModel> GetByIdSaveViewModel(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RealizarTransferenciaAsync(TransaccionesViewModel model)
        {
            // Validamos que el usuario esté autenticado
            if (userViewModel == null)
            {
                throw new UnauthorizedAccessException("El usuario no está autenticado.");
            }

            // Validamos que las cuentas de origen y destino existen
            var cuentaOrigen = await _cuentasAhorroRepository.GetByIdAsync(model.CuentaOrigenId);
            var cuentaDestino = await _cuentasAhorroRepository.GetByIdAsync(model.CuentaDestinoId);

            if (cuentaOrigen == null || cuentaDestino == null)
            {
                throw new ArgumentException("Una o ambas cuentas especificadas no existen.");
            }

            // Verificamos que la cuenta origen tiene saldo suficiente
            if (cuentaOrigen.Balance < model.Monto)
            {
                throw new InvalidOperationException("Saldo insuficiente en la cuenta de origen.");
            }

            // Mapear el modelo de vista a la entidad de transacción
            var transaccion = _mapper.Map<Transacciones>(model);
            transaccion.IdUsuario = userViewModel.Id; // Asigna el ID del usuario autenticado
            transaccion.Fecha = DateTime.Now;

            // Actualizamos los saldos de las cuentas
            cuentaOrigen.Balance -= model.Monto;
            cuentaDestino.Balance += model.Monto;

            // Guardamos los cambios en las cuentas y en la transacción
            await _cuentasAhorroRepository.UpdateAsync(cuentaOrigen);
            await _cuentasAhorroRepository.UpdateAsync(cuentaDestino);
            await _transaccionesRepository.AddAsync(transaccion);

            return true;
        }

        public Task Update(TransaccionesViewModel vm, int id)
        {
            throw new NotImplementedException();
        }
    }
}
