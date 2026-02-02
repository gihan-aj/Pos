using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderValidator : AbstractValidator<ConfirmOrderCommand>
    {
        public ConfirmOrderValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
