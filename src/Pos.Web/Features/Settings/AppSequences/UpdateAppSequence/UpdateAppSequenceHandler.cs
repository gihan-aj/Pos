using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Settings.AppSequences.UpdateAppSequence;

public class UpdateAppSequenceHandler : ICommandHandler<UpdateAppSequenceCommand>
{
    private readonly AppDbContext _context;

    public UpdateAppSequenceHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateAppSequenceCommand command, CancellationToken cancellationToken)
    {
        var sequence = await _context.AppSequences
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken);

        if (sequence == null)
        {
            return Result.Failure(Error.NotFound("Sequence.NotFound", $"Sequence '{command.Id}' not found."));
        }

        string nextGeneratedNumber = $"{command.Prefix}{command.CurrentValue + command.Increment}";
        bool alreadyExists = false;

        if (command.Id == "Order")
        {
            alreadyExists = await _context.Orders.AnyAsync(o => o.OrderNumber == nextGeneratedNumber, cancellationToken);
        }
        else if (command.Id == "Sku")
        {
            alreadyExists = await _context.Products.AnyAsync(p => p.Sku == nextGeneratedNumber, cancellationToken);
        }

        if (alreadyExists)
        {
            return Result.Failure(Error.Conflict("Sequence.Conflict", 
                $"The next generated number '{nextGeneratedNumber}' already exists in the system. Please choose a higher starting value."));
        }

        sequence.Prefix = command.Prefix;
        sequence.CurrentValue = command.CurrentValue;
        sequence.Increment = command.Increment;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
