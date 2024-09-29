using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using test_tesk_rest_solution.Handlers.OrderController.GetOrderList;
using test_tesk_rest_solution.Services.Interfaces;
using test_tesk_rest_solution.ViewModels;

namespace test_tesk_rest_solution;

public class GetOrderListHandler(IMapperBase mapper, IOrderRepository repository) : IRequestHandler<GetOrderListRequest, 
    GetOrderListResponse>
{
    public async Task<GetOrderListResponse> Handle(GetOrderListRequest request, CancellationToken cancellationToken)
    {
        var order = await repository.GetPendingOrdersSortedByPriorityAsync();

        var models = order.Select(mapper.Map<OrderViewModel>).ToList();
        
        var response = new GetOrderListResponse
        {
            Message = "Order list have been successfully received.",
            StatusCode = StatusCodes.Status200OK,
            Total = models.Count,
            Elements = models
        };

        return response;
    }
}