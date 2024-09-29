using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test_tesk_rest_solution.Data;
using test_tesk_rest_solution.Data.Entities;
using test_tesk_rest_solution.Data.Entities.Enums;
using test_tesk_rest_solution.Services.Interfaces;

namespace test_tesk_rest_solution.Services.Implementations;

public class OrderRepository(OrdersDbContext context) : IOrderRepository
{
    public async Task<OrderEntity> GetOrderByIdAsync(int id)
    {
        return await context.Orders.FindAsync(id);
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByStatusAsync(StatusType status)
    {
        return await context.Orders
            .Where(o => o.Status == status)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<OrderEntity>> GetPendingOrdersSortedByPriorityAsync()
    {
        return await context.Orders
            .Where(o => o.Status == StatusType.Pending)
            .OrderByDescending(o => o.Priority)
            .ToListAsync();
    }

    public async Task AddOrderAsync(OrderEntity order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(OrderEntity order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
    }
}