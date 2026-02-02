using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.VoidPayment
{
    public class VoidPaymentValidator : AbstractValidator<VoidPaymentCommand>
    {
        public VoidPaymentValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.PaymentId).NotEmpty();
        }
    }
}
