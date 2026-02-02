using FluentValidation;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.RefundOrderPayment
{
    public record RefundOrderPaymentCommand(Guid OrderId, decimal Amount, string Reason) : ICommand;
    public class RefundOrderPaymentValidator : AbstractValidator<RefundOrderPaymentCommand>
    {
        public RefundOrderPaymentValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Refund amount must be greater than zero.");
            RuleFor(x => x.Reason).NotEmpty().WithMessage("Refund reason is required.");
        }
    }

    public class RefundOrderPaymentHandler : ICommandHandler<RefundOrderPaymentCommand>
    {
        private readonly AppDbContext _dbContext;

        public RefundOrderPaymentHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Result> Handle(RefundOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
