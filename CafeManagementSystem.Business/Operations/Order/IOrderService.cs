using System;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Types;

namespace CafeManagementSystem.Business.Operations.Order
{
	public interface IOrderService
	{
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto dto);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetOrdersByUserAsync(int userId);
        Task DeleteOrderAsync(int id);
        Task<ServiceMessage> AddjustIsConfirmed(int id, bool changeTo);
        
    }
}

