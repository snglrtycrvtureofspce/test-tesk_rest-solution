using MediatR;
using test_tesk_rest_solution.Data.Entities.Enums;

namespace test_tesk_rest_solution.Handlers.OrderController.CreateOrder;

public class CreateOrderRequest : IRequest<CreateOrderResponse>
{
    public string CustomerName { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public CurrencyType Currency { get; set; }
}