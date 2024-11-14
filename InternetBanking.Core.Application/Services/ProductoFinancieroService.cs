using AutoMapper;
using InternetBanking.Core.Application.DTOS.ProductosFinacieros;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Repositories.Generic;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.ProductosFinancieros;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.Services;

public class ProductoFinancieroService : IProductoFinancieroService
{
    private readonly IProductoFinancieroRepository _productoRepository;

    private readonly IMapper _mapper;

    public ProductoFinancieroService(IProductoFinancieroRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;

    }

    public async Task<ProductoFinancieroResponse> CrearCuentaAhorroAsync(decimal balanceInicial, string userId)
    {
        // Crear el producto financiero principal
        var productoFinanciero = new ProductoFinanciero
        {
            IdentificadorUnico = GenerateUniqueIdentifier(),
            IdUsuario = userId,
            FechaCreacion = DateTime.UtcNow,
            CuentasAhorro = new List<CuentasAhorro>()
        };

        // Crear la cuenta de ahorro asociada
        var cuentaAhorro = new CuentasAhorro
        {
            IdentificadorUnico = GenerateUniqueIdentifier(),
            NumeroCuenta = GenerateAccountNumber(),
            Balance = balanceInicial,
            EsPrincipal = true,
            ProductoFinanciero = productoFinanciero
        };

        productoFinanciero.CuentasAhorro.Add(cuentaAhorro);

        // Guardar en el repositorio
        await _productoRepository.AddAsync(productoFinanciero);

        return new ProductoFinancieroResponse
        {
            IdentificadorUnico = cuentaAhorro.IdentificadorUnico,
            TipoProducto = TipoProducto.CuentaAhorro,
            BalanceInicial = balanceInicial,
            Message = "Cuenta de Ahorro creada exitosamente"
        };
    }

    public async Task<List<ProductosFinancierosViewModel>> GetProductosPorClienteAsync(int clienteId)
    {
        // Llamar al repositorio para obtener los productos financieros del usuario autenticado
        var productos = await _productoRepository.GetByIdAsync(clienteId);

        // Mapear los resultados a la vista del modelo
        return _mapper.Map<List<ProductosFinancierosViewModel>>(productos);
    }

    private string GenerateUniqueIdentifier()
    {
        return new Random().Next(100000000, 999999999).ToString();
    }

    private string GenerateAccountNumber()
    {
        return "AH" + new Random().Next(10000000, 99999999).ToString();
    }
}


