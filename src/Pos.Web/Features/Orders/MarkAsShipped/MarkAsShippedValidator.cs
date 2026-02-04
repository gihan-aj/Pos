using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.MarkAsShipped
{
    public class MarkAsShippedValidator : AbstractValidator<MarkAsShippedCommand>
    {
        public MarkAsShippedValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
