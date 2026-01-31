using FluentValidation;

namespace Pos.Web.Features.Orders.RemoveOrderItem
{
    public class RemoveOrderItemValidator : AbstractValidator<RemoveOrderItemCommand>
    {
        public RemoveOrderItemValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.OrderItemId).NotEmpty();
        }
    }
}
