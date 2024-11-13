using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Productos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICuentaAhorroRepository _cuentaAhorroRepository;
        private readonly ITarjetaCreditoRepository _tarjetaCreditoRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IMapper _mapper;

        public ProductoService(
            IProductoRepository productoRepository, 
            ICuentaAhorroRepository cuentaAhorroRepository, 
            ITarjetaCreditoRepository tarjetaCreditoRepository, 
            IPrestamoRepository prestamoRepository, 
            IMapper mapper)
        {
            _productoRepository = productoRepository;
            _cuentaAhorroRepository = cuentaAhorroRepository;
            _tarjetaCreditoRepository = tarjetaCreditoRepository;
            _prestamoRepository = prestamoRepository;
            _mapper = mapper;
        }

        public async Task<ProductoViewModel> AddProducto(ProductoSaveViewModel saveViewModel)
        {
            var producto = _mapper.Map<Producto>(saveViewModel);

            // Lógica para generar un ID único de 9 dígitos
            producto.NumeroIdentificador = GenerateUniqueProductIdentifier();

            // Validación específica de tipo de producto
            if (producto is CuentasAhorro)
            {
                // Se puede agregar lógica para las Cuentas de Ahorro
            }
            else if (producto is TarjetaCredito)
            {
                // Validar límite de crédito
            }
            else if (producto is Prestamo)
            {
                // Validar monto del préstamo
            }

            await _productoRepository.AddAsync(producto);
            return _mapper.Map<ProductoViewModel>(producto);
        }

        public async Task DeleteProducto(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);

            // Lógica para eliminar producto, por ejemplo, validación de saldo
            if (producto is CuentaAhorro cuentaAhorro)
            {
                if (cuentaAhorro.EsCuentaPrincipal || cuentaAhorro.Balance > 0)
                {
                    throw new InvalidOperationException("No se puede eliminar la cuenta principal o con saldo.");
                }
            }

            if (producto is TarjetaCredito tarjetaCredito || producto is Prestamo prestamo)
            {
                if (tarjetaCredito.BalanceDeuda > 0 || prestamo.BalanceDeuda > 0)
                {
                    throw new InvalidOperationException("No se puede eliminar el producto con deuda.");
                }
            }

            await _productoRepository.DeleteAsync(producto);
        }

        private string GenerateUniqueProductIdentifier()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999).ToString(); // Genera un número de 9 dígitos
        }
    }
}
