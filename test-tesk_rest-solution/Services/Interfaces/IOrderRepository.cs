using System.Collections.Generic;
using System.Threading.Tasks;
using test_tesk_rest_solution.Data.Entities;
using test_tesk_rest_solution.Data.Entities.Enums;

namespace test_tesk_rest_solution.Services.Interfaces;

public interface IOrderRepository
{
    Task<OrderEntity> GetOrderByIdAsync(int id);
    
    Task<IEnumerable<OrderEntity>> GetPendingOrdersSortedByPriorityAsync();
    
    Task<IEnumerable<OrderEntity>> GetOrdersByStatusAsync(StatusType status);
    
    Task AddOrderAsync(OrderEntity order);
    
    Task UpdateOrderAsync(OrderEntity order);
}