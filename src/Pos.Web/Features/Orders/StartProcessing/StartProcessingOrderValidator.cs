using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.StartProcessing
{
    public class StartProcessingOrderValidator : AbstractValidator<StartProcessingOrderCommand>
    {
        public StartProcessingOrderValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
