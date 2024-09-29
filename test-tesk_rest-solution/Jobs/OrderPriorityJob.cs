using System;
using System.Threading.Tasks;
using test_tesk_rest_solution.Data.Entities;
using test_tesk_rest_solution.Data.Entities.Enums;
using test_tesk_rest_solution.Services.Interfaces;

namespace test_tesk_rest_solution.Jobs;

public class OrderPriorityJob(IOrderRepository orderRepository)
{
    public async Task RecalculateOrderPrioritiesAsync()
    {
        var pendingOrders = await orderRepository.GetOrdersByStatusAsync(StatusType.Pending);

        foreach (var order in pendingOrders)
        {
            order.Priority = RecalculatePriority(order);
            await orderRepository.UpdateOrderAsync(order);
        }
    }

    private static int RecalculatePriority(OrderEntity order)
    {
        var priority = (int)order.TotalAmount;
        var minutesSinceCreation = (DateTime.UtcNow - order.OrderDate).TotalMinutes;
        priority += (int)(minutesSinceCreation / 10);
        var hoursSinceCreation = (DateTime.UtcNow - order.OrderDate).TotalHours;
        priority += (int)hoursSinceCreation;

        return priority;
    }
}