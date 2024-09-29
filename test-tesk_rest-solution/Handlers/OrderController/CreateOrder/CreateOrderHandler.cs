using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using test_tesk_rest_solution.Data.Entities;
using test_tesk_rest_solution.Data.Entities.Enums;
using test_tesk_rest_solution.Handlers.OrderController.CreateOrder;
using test_tesk_rest_solution.Services.Interfaces;
using test_tesk_rest_solution.ViewModels;

namespace test_tesk_rest_solution;

public class CreateOrderHandler(IOrderRepository orderRepository, IMapperBase mapper) : 
    IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new OrderEntity
        {
            CustomerName = request.CustomerName,
            OrderDate = DateTime.UtcNow,
            TotalAmount = request.TotalAmount,
            Currency = request.Currency,
            Status = StatusType.Pending
            /*Priority = CalculatePriority(request.TotalAmount, DateTime.UtcNow)*/
        };

        await orderRepository.AddOrderAsync(order);

        var model = mapper.Map<OrderViewModel>(order);
        
        var response = new CreateOrderResponse
        {
            Message = "Order have been successfully created.",
            StatusCode = StatusCodes.Status201Created,
            Item = model
        };

        return response;
    }
    
    /*private static int CalculatePriority(decimal totalAmount, DateTime orderDate)
    {
        var priority = (int)totalAmount;
        
        var minutesSinceCreation = (DateTime.UtcNow - orderDate).TotalMinutes;
        priority += (int)(minutesSinceCreation / 10);

        return priority;
    }*/
}