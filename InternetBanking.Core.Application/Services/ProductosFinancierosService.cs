using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;


namespace InternetBanking.Core.Application.Services
{
    public class ProductosFinancierosService : GenericService<SaveProductosFinancierosViewModel, ProductosFinancierosViewModel, ProductosFinancieros>, IProductosFinancierosService
    {
        private readonly IProductosFinancierosRepository _productosFinancierosRepository;
        private readonly ICuentasAhorroRepository _cuentasAhorroRepository;
        private readonly ITarjetasCreditoRepository _tarjetasCreditoRepository;
        private readonly IPrestamosRepository _prestamosRepository;
        private readonly IMapper _mapper;
        public ProductosFinancierosService(
            IProductosFinancierosRepository productosFinancierosRepository,
            ICuentasAhorroRepository cuentasAhorroRepository,
            ITarjetasCreditoRepository tarjetasCreditoRepository,
            IPrestamosRepository prestamosRepository,
            IMapper mapper) : base(productosFinancierosRepository, mapper)
        {
            _productosFinancierosRepository = productosFinancierosRepository;
            _cuentasAhorroRepository = cuentasAhorroRepository;
            _tarjetasCreditoRepository = tarjetasCreditoRepository;
            _prestamosRepository = prestamosRepository;
            _mapper = mapper;
        }
        public async Task<SaveProductosFinancierosViewModel> AddProductoAsync(SaveProductosFinancierosViewModel model)
        {
            // Verificar que el identificador único sea único en el sistema
            var existingProduct = await _productosFinancierosRepository.GetByIdentificadorUnicoAsync(model.IdentificadorUnico);
            if (existingProduct != null)
            {
                throw new Exception("El identificador único ya está registrado.");
            }

            // Crear el producto financiero
            var producto = new ProductosFinancieros
            {
                IdentificadorUnico = model.IdentificadorUnico, // Asegúrate de que el modelo tenga esta propiedad
                IdUsuario = model.IdUsuario,
                TipoProducto = model.TipoProducto,
                FechaCreacion = DateTime.Now
            };

            // Lógica especial para tipo de producto
            if (producto.TipoProducto == TipoProducto.CuentaAhorro)
            {
                // Si es una cuenta de ahorro, crearla
                var cuenta = new CuentasAhorro
                {
                    IdentificadorUnico = producto.IdentificadorUnico,
                    EsPrincipal = model.EsPrincipal ?? false, // Asegúrate de manejar la propiedad correctamente
                    Balance = model.Balance ?? 0, // Asegúrate de que Balance no sea nulo
                    ProductoFinanciero = producto
                };

                await _cuentasAhorroRepository.AddAsync(cuenta);
            }
            else if (producto.TipoProducto == TipoProducto.TarjetaCredito)
            {
                // Si es tarjeta de crédito, crearla
                var tarjeta = new TarjetasCredito
                {
                    IdentificadorUnico = producto.IdentificadorUnico,
                    LimiteCredito = model.LimiteCredito ?? 0, // Asegúrate de que el límite de crédito no sea nulo
                    ProductoFinanciero = producto
                };

                await _tarjetasCreditoRepository.AddAsync(tarjeta);
            }
            else if (producto.TipoProducto == TipoProducto.Prestamo)
            {
                // Si es préstamo, crear
                var prestamo = new Prestamos
                {
                    MontoPrestamo = model.MontoPrestamo ?? 0, // Asegúrate de que el monto del préstamo no sea nulo
                    DeudaRestante = model.MontoPrestamo ?? 0,
                    ProductoFinanciero = producto
                };

                // Sumar el monto del préstamo al balance de la cuenta principal
                var cuentaPrincipal = await _cuentasAhorroRepository.GetPrimaryAccountByUserIdAsync(model.IdUsuario);
                if (cuentaPrincipal != null)
                {
                    cuentaPrincipal.Balance += model.MontoPrestamo ?? 0;
                    await _cuentasAhorroRepository.CuentasAhorroUpdateAsync(cuentaPrincipal);
                }

                await _prestamosRepository.AddAsync(prestamo);
            }

            // Crear el producto financiero
            await _productosFinancierosRepository.AddAsync(producto);
            return _mapper.Map<SaveProductosFinancierosViewModel>(producto);
        }

        public async Task<bool> DeleteProductoAsync(string identificadorUnico)
        {
            var producto = await _productosFinancierosRepository.GetByIdentificadorUnicoAsync(identificadorUnico);
            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }

            // Validar que el producto no tenga deuda antes de eliminarlo
            if (producto.TipoProducto == TipoProducto.Prestamo)
            {
                var prestamos = await _prestamosRepository.GetByUserIdAsync(producto.IdUsuario);
                if (prestamos.Any(p => p.DeudaRestante > 0))
                {
                    throw new Exception("No se puede eliminar un producto de préstamo con saldo pendiente.");
                }
            }
            else if (producto.TipoProducto == TipoProducto.TarjetaCredito)
            {
                var tarjeta = await _tarjetasCreditoRepository.GetByIdentificadorUnicoAsync(identificadorUnico);
                if (tarjeta.DeudaActual > 0)
                {
                    throw new Exception("No se puede eliminar una tarjeta de crédito con deuda.");
                }
            }
            else if (producto.TipoProducto == TipoProducto.CuentaAhorro)
            {
                var cuenta = await _cuentasAhorroRepository.GetSavingsAccountByUserIdAsync(producto.IdUsuario);
                if (cuenta.Balance > 0 && !cuenta.EsPrincipal)
                {
                    throw new Exception("No se puede eliminar una cuenta de ahorro con saldo.");
                }
            }

            // Eliminar el producto financiero, pasando el objeto completo
            return await _productosFinancierosRepository.DeleteAsync(producto);
        }
        public async Task<List<ProductosFinancierosViewModel>> GetProductosByUserIdAsync(string userId)
        {
            var productos = await _productosFinancierosRepository.GetByUserIdAsync(userId);

            var productosViewModel = _mapper.Map<List<ProductosFinancierosViewModel>>(productos);

            return productosViewModel;
        }


    }
}
