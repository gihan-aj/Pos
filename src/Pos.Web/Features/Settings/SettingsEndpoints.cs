using Pos.Web.Features.Settings.AppSequences.GetAppSequences;
using Pos.Web.Features.Settings.AppSequences.UpdateAppSequence;

namespace Pos.Web.Features.Settings;

public static class SettingsEndpoints
{
    public static void MapSettingsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/settings")
            .WithTags("Settings");

        group.MapGetAppSequences();
        group.MapUpdateAppSequence();
    }
}
