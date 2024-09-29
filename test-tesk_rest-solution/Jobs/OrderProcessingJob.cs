using System;
using System.IO;
using System.Threading.Tasks;
using test_tesk_rest_solution.Data.Entities;
using test_tesk_rest_solution.Data.Entities.Enums;
using test_tesk_rest_solution.Services.Interfaces;

namespace test_tesk_rest_solution.Jobs;

public class OrderProcessingJob(IOrderRepository orderRepository, ICurrencyConverterService currencyConverterService)
{
    public async Task ProcessPendingOrdersAsync()
    {
        var pendingOrders = await orderRepository.GetPendingOrdersSortedByPriorityAsync();

        foreach (var order in pendingOrders)
        {
            try
            {
                order.Status = StatusType.Processing;
                await orderRepository.UpdateOrderAsync(order);

                var conversionRate =
                    await currencyConverterService.GetConversionRateAsync(order.Currency, CurrencyType.Usd);

                if (conversionRate == null)
                {
                    order.Status = StatusType.Pending;
                }
                else
                {
                    order.TotalAmountInBaseCurrency = order.TotalAmount * conversionRate.Value;
                    order.Status = StatusType.Completed;

                    LogOrderInfoToFile(order);
                }

                await orderRepository.UpdateOrderAsync(order);
            }
            catch (Exception)
            {
                order.Status = StatusType.Pending;
                await orderRepository.UpdateOrderAsync(order);
            }
        }
    }

    private static void LogOrderInfoToFile(OrderEntity order)
    {
        var logMessage = $"Time: {DateTime.UtcNow}, Order ID: {order.Id}, Customer: {order.CustomerName}, " +
                         $"Total Amount: {order.TotalAmount} {order.Currency}, " +
                         $"Amount in Base Currency: {order.TotalAmountInBaseCurrency} USD";

        File.AppendAllText("order_log.txt", logMessage + Environment.NewLine);
    }
}