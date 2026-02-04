using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.MarkAsDelivered
{
    public class MarkAsDeliveredValidator : AbstractValidator<MarkAsDeliveredCommand>
    {
        public MarkAsDeliveredValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
