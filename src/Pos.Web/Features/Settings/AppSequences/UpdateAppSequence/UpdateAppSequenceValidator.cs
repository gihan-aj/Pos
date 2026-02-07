using FluentValidation;

namespace Pos.Web.Features.Settings.AppSequences.UpdateAppSequence;

public class UpdateAppSequenceValidator : AbstractValidator<UpdateAppSequenceCommand>
{
    public UpdateAppSequenceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Prefix)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Increment)
            .GreaterThan(0);
    }
}
