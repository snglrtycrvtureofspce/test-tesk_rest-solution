using MediatR;

namespace test_tesk_rest_solution.Handlers.OrderController.CancelOrder;

public class CancelOrderRequest : IRequest<CancelOrderResponse>
{
    public int Id { get; init; }
}