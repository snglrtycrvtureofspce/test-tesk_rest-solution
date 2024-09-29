using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using test_tesk_rest_solution.Services.Interfaces;
using test_tesk_rest_solution.ViewModels;

namespace test_tesk_rest_solution.Handlers.OrderController.GetOrder;

public class GetOrderHandler(IOrderRepository orderRepository, IMapperBase mapper) : 
    IRequestHandler<GetOrderRequest, GetOrderResponse>
{
    public async Task<GetOrderResponse> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderByIdAsync(request.Id);
        
        var model = mapper.Map<OrderViewModel>(order);

        var response = new GetOrderResponse
        {
            Message = "Order have been successfully received.",
            StatusCode = StatusCodes.Status200OK,
            Item = model
        };

        return response;
    }
}