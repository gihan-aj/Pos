using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.MarkAsReadyToShip
{
    public class MarkAsReadyToShipValidator : AbstractValidator<MarkAsReadyToShipCommand>
    {
        public MarkAsReadyToShipValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
