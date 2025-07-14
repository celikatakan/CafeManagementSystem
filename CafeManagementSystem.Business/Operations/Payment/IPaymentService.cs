using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Order.Dtos;

namespace CafeManagementSystem.Business.Operations.Payment
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutFormAsync(OrderDto order, decimal totalAmount);
    }
} 