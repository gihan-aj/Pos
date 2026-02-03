using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.CompleteProcess
{
    public class CompleteProcessingOrderValidator : AbstractValidator<CompleteProcessingOrderCommand>
    {
        public CompleteProcessingOrderValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
