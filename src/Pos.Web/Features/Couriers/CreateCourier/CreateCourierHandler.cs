using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Couriers.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Couriers.CreateCourier
{
    public class CreateCourierHandler : ICommandHandler<CreateCourierCommand, Guid>
    {
        private readonly AppDbContext _appDbContext;

        public CreateCourierHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<Guid>> Handle(CreateCourierCommand command, CancellationToken cancellationToken)
        {
            bool courierNameExists = await _appDbContext.Couriers
                .AnyAsync(c => c.Name == command.Name, cancellationToken);

            if (courierNameExists)
                return Result.Failure<Guid>(Error.Conflict("Courier.AlreadyExists", $"A courier with the name '{command.Name}' already exists."));

            var createResult = Courier.Create(
                command.Name,
                command.TrackingUrlTemplate,
                command.WebsiteUrl,
                command.PhoneNumber);

            if(createResult.IsFailure)
                return Result.Failure<Guid>(createResult.Error);

            var courier = createResult.Value;

            _appDbContext.Couriers.Add(courier);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return courier.Id;
        }
    }
}
