using MediatR;

namespace test_tesk_rest_solution.Handlers.OrderController.GetOrder;

public class GetOrderRequest : IRequest<GetOrderResponse>
{
    public int Id { get; set; }
}