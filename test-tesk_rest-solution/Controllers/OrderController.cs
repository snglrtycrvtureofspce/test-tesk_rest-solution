using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using test_tesk_rest_solution.Handlers.OrderController.CancelOrder;
using test_tesk_rest_solution.Handlers.OrderController.CreateOrder;
using test_tesk_rest_solution.Handlers.OrderController.GetOrder;
using test_tesk_rest_solution.Handlers.OrderController.GetOrderList;

namespace test_tesk_rest_solution.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class OrderController(ISender sender) : ControllerBase
{
    /// <summary>
    /// The method provider possibility to create an order class item.
    /// </summary>
    /// <param name="request">The request object containing the details of the order class to be created.</param>
    /// <returns></returns>
    [HttpPost(Name = "CreateOrder")]
    [SwaggerResponse(statusCode: StatusCodes.Status201Created, type: typeof(CreateOrderResponse))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request) => 
        Ok(await sender.Send(request));
    
    /// <summary>
    /// The method provider possibility to get order by id.
    /// </summary>
    /// <param name="id">Identifier of the order class to be received.</param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "GetOrder")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetOrderResponse))]
    public async Task<IActionResult> GetOrder(int id) => Ok(await sender.Send(new GetOrderRequest { Id = id }));

    /// <summary>
    /// The method provider possibility to receive an order list.
    /// </summary>
    [HttpGet(Name = "GetOrderList")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetOrderListResponse))]
    public async Task<IActionResult> GetOrderList() => Ok(await sender.Send(new GetOrderListRequest()));
    
    /// <summary>
    /// The method provider possibility to cancel an order class by id.
    /// </summary>
    /// <param name="id">Identifier of the order class to be received.</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = "CancelOrder")]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CancelOrderResponse))]
    public async Task<IActionResult> CancelOrder(int id) => Ok(await sender.Send(new CancelOrderRequest { Id = id }));
}