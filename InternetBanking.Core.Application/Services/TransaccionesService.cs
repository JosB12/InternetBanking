using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Repositories.Transaccion;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels.Transacciones;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using Microsoft.Extensions.Logging;

public class TransaccionesService : GenericService<TransaccionesSaveViewModel, TransaccionesViewModel, Transacciones>, ITransaccionesService
{
    private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
    protected readonly IMapper _mapper;
    private readonly ILogger<TransaccionesService> _logger;

    public TransaccionesService(ITransaccionesRepository repository, IMapper mapper, ICuentasAhorroRepository cuentasAhorroRepository, ILogger<TransaccionesService> logger)
        : base(repository, mapper)
    {
        _cuentasAhorroRepository = cuentasAhorroRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> TransferirFondos(TransferenciaViewModel transferenciaVm)
    {
        var cuentaOrigen = await _cuentasAhorroRepository.GetByIdAsync(transferenciaVm.IdCuentaOrigen);
        var cuentaDestino = await _cuentasAhorroRepository.GetByIdAsync(transferenciaVm.IdCuentaDestino);

        if (cuentaOrigen == null || cuentaDestino == null)
        {
            _logger.LogError("Cuenta origen o destino no encontrada.");
            return false; // Error: cuentas no encontradas.
        }

        if (cuentaOrigen.Balance < transferenciaVm.Monto)
        {
            _logger.LogError($"Saldo insuficiente en la cuenta origen. Saldo actual: {cuentaOrigen.Balance}, Monto requerido: {transferenciaVm.Monto}");
            return false; // Error: saldo insuficiente.
        }

        // Verificar que las cuentas están relacionadas con ProductosFinancieros
        var productoFinancieroOrigen = await _cuentasAhorroRepository.GetByProductoIdAsync(cuentaOrigen.IdProductoFinanciero);
        var productoFinancieroDestino = await _cuentasAhorroRepository.GetByProductoIdAsync(cuentaDestino.IdProductoFinanciero);

        if (productoFinancieroOrigen == null || productoFinancieroDestino == null)
        {
            _logger.LogError("Las cuentas no están relacionadas con ProductosFinancieros.");
            return false; // Error: las cuentas no están relacionadas con ProductosFinancieros.
        }

        // Restar monto en cuenta origen y agregarlo en cuenta destino
        cuentaOrigen.Balance -= transferenciaVm.Monto;
        cuentaDestino.Balance += transferenciaVm.Monto;

        // Actualizar en la base de datos
        await _cuentasAhorroRepository.CuentasAhorroUpdateAsync(cuentaOrigen);
        await _cuentasAhorroRepository.CuentasAhorroUpdateAsync(cuentaDestino);

        // Registrar la transacción
        var transaccion = new Transacciones
        {
            IdUsuario = transferenciaVm.IdUsuario,
            TipoTransaccion = TipoTransaccion.Transferencia,
            Monto = transferenciaVm.Monto,
            Fecha = DateTime.Now,
            IdCuentaOrigen = transferenciaVm.IdCuentaOrigen,
            IdCuentaDestino = transferenciaVm.IdCuentaDestino,
            IdProductoFinanciero = cuentaOrigen.IdProductoFinanciero // Ensure this is set correctly
        };

        await Add(_mapper.Map<TransaccionesSaveViewModel>(transaccion));
        return true;
    }

    public async Task<List<MostrarCuentaAhorroViewModel>> GetCuentasByUserIdAsync(string userId)
    {
        var cuentas = await _cuentasAhorroRepository.GetByUserIdAsync(userId);
        return _mapper.Map<List<MostrarCuentaAhorroViewModel>>(cuentas);
    }
}
