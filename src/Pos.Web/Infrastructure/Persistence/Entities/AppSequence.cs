using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Infrastructure.Persistence.Entities;

public class AppSequence
{
    public string Id { get; set; } = null!;
    public string Prefix { get; set; } = null!;
    public int CurrentValue { get; set; }
    public int Increment { get; set; }
    public byte[] RowVersion { get; set; } = null!;
}
