using FluentValidation;

namespace test_tesk_rest_solution.Handlers.OrderController.CreateOrder;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName cannot be empty");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("TotalAmount must be greater than zero");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Unsupported currency type");
    }
}