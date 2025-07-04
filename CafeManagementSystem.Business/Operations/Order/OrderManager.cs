using System;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystem.Business.Operations.Order
{
    public class OrderManager : IOrderService
	{
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(
            IRepository<OrderEntity> orderRepository,
            IRepository<ProductEntity> productRepository,
            IRepository<UserEntity> userRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Validations
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.GuestCount < 1) throw new ArgumentException("Geçersiz misafir sayısı");

            var product = _productRepository.GetById(dto.ProductId);
            if (product == null) throw new KeyNotFoundException("Ürün bulunamadı");

            // Stok kontrolü (StockQuantity null ise kontrol yapma)
            if (product.StockQuantity.HasValue && product.StockQuantity < dto.GuestCount)
                throw new InvalidOperationException($"Yetersiz stok. Mevcut: {product.StockQuantity}, İstenen: {dto.GuestCount}");

            var user = _userRepository.GetById(dto.UserId);
            if (user == null) throw new KeyNotFoundException("Kullanıcı bulunamadı");

            // Create order
            var order = new OrderEntity
            {
                ProductId = dto.ProductId,
                UserId = dto.UserId,
                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,
                GuestCount = dto.GuestCount,
                SpecialRequest = dto.SpecialRequest,
                IsConfirmed = false
            };

            _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();

            // Refresh the order with includes
            var fullOrder = await _orderRepository.GetAll(o => o.Id == order.Id)
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync();


            return new OrderDto(
        Id: fullOrder.Id,
        ProductId: fullOrder.ProductId,
        ProductName: fullOrder.Product?.ProductName
                    ?? (_productRepository.GetById(fullOrder.ProductId))?.ProductName
                    ?? "Ürün adı yok",
        UserId: fullOrder.UserId,
        UserFullName: fullOrder.User != null
                    ? $"{fullOrder.User.FirstName} {fullOrder.User.LastName}"
                    : (_userRepository.GetById(fullOrder.UserId)) is UserEntity u
                        ? $"{u.FirstName} {u.LastName}"
                        : "Kullanıcı bilgisi yok",
        CreatedDate: fullOrder.CreatedDate,
        GuestCount: fullOrder.GuestCount,
        IsConfirmed: fullOrder.IsConfirmed,
        SpecialRequest: fullOrder.SpecialRequest
    );
        }

        public async Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto dto)
        {
            var order = await _orderRepository.GetAll(o => o.Id == id)
                .Include(o => o.Product) // Eager loading ekleyin
                .Include(o => o.User)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new ArgumentException("Sipariş bulunamadı");

            // Güncelleme işlemleri
            if (dto.IsConfirmed.HasValue)
                order.IsConfirmed = dto.IsConfirmed.Value;

            // ... diğer güncellemeler

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(
                order,
                order.Product?.ProductName ?? "Ürün bilgisi yok",
                order.User != null ? $"{order.User.FirstName} {order.User.LastName}" : "Kullanıcı bilgisi yok"
            );
        }

        private static OrderDto MapToDto(OrderEntity order, string productName, string userFullName) =>
            new(
                order.Id,
                order.ProductId,
                productName,
                order.UserId,
                userFullName,
                order.CreatedDate,
                order.GuestCount,
                order.IsConfirmed,
                order.SpecialRequest
            );

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository
        .GetAll(o => o.Id == id)
        .Include(o => o.Product) // Product bilgisini joinle
        .Include(o => o.User)    // User bilgisini joinle
        .Select(o => new OrderDto(
            o.Id,
            o.ProductId,
            o.Product.ProductName,
            o.UserId,
            $"{o.User.FirstName} {o.User.LastName}",
            o.CreatedDate,
            o.GuestCount,
            o.IsConfirmed,
            o.SpecialRequest
        ))
        .FirstOrDefaultAsync();

            return order ?? throw new KeyNotFoundException($"Order with ID {id} not found");
        }

        public async Task<List<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            return await _orderRepository
         .GetAll(o => o.UserId == userId)
         .Include(o => o.Product)
         .Include(o => o.User)
         .Select(o => new OrderDto(
             o.Id,
             o.ProductId,
             o.Product.ProductName,
             o.UserId,
             $"{o.User.FirstName} {o.User.LastName}",
             o.CreatedDate,
             o.GuestCount,
             o.IsConfirmed,
             o.SpecialRequest
         ))
         .ToListAsync();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _orderRepository
                .GetAll()
                .Include(o => o.Product)
                .Include(o => o.User)
                .Select(o => new OrderDto(
                    o.Id,
                    o.ProductId,
                    o.Product.ProductName,
                    o.UserId,
                    $"{o.User.FirstName} {o.User.LastName}",
                    o.CreatedDate,
                    o.GuestCount,
                    o.IsConfirmed,
                    o.SpecialRequest
                ))
                .ToListAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetAll(o => o.Id == id).FirstOrDefaultAsync();
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            _orderRepository.Delete(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ServiceMessage> AddjustIsConfirmed(int id, bool changeTo)
        {
            var order = _orderRepository.GetById(id);

            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kafe bulunamadı."
                };
            }
            order.IsConfirmed = changeTo;

            _orderRepository.Update(order);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Sipariş onaylanırken hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Sipariş Başarıyla onaylandı."
            };
        }
    }
}

