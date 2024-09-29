using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using test_tesk_rest_solution.Data;
using test_tesk_rest_solution.Data.Entities.Enums;
using test_tesk_rest_solution.Handlers.OrderController.CancelOrder;
using test_tesk_rest_solution.ViewModels;

namespace test_tesk_rest_solution;

public class CancelOrderCommandHandler(OrdersDbContext context, IMapperBase mapper) : 
    IRequestHandler<CancelOrderRequest, CancelOrderResponse>
{
    public async Task<CancelOrderResponse> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var order = 
            await context.Orders.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        if (order == null || order.Status == StatusType.Completed)
        {
            return new CancelOrderResponse
            {
                Message = "An order cannot be canceled.",
                StatusCode = StatusCodes.Status200OK
            };
        }

        order.Status = StatusType.Cancelled;
        await context.SaveChangesAsync(cancellationToken);

        var model = mapper.Map<OrderViewModel>(order);

        var response = new CancelOrderResponse
        {
            Message = "Order have been successfully cancelled.",
            StatusCode = StatusCodes.Status200OK,
            Item = model
        };

        return response;
    }
}