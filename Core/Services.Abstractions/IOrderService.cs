using Shared.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task<OrderResultDto> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResultDto>>GetOrdersByEmailAsync(string useremail);
        Task<OrderResultDto>CreateOrderAsync(OrderRequestDto orderRequest,string userEmail);
        // Get All Delivery Method
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync();
    }
}
