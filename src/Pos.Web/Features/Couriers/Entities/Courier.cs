using Pos.Web.Features.Orders.Entities;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Couriers.Entities
{
    public sealed class Courier : AuditableEntity
    {
        private Courier() { }

        private Courier(string name, string? websiteUrl, string? trackingUrlTemplate, string? phoneNumber)
        {
            Id = Guid.NewGuid();
            Name = name;
            WebsiteUrl = websiteUrl;
            TrackingUrlTemplate = trackingUrlTemplate;
            PhoneNumber = phoneNumber;
            IsActive = true;
        }

        public string Name { get; set; } = string.Empty;

        // Useful for links: "https://track.dhl.com/?id="
        public string? TrackingUrlTemplate { get; private set; }

        public string? WebsiteUrl { get; private set; }
        public string? PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<Order> _orders = new List<Order>();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        // --- FACTORY ---
        public static Result<Courier> Create(string name, string? websiteUrl = null, string? trackingUrlTemplate = null, string? contactNumber = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Courier>(new Shared.Errors.Error("Courier.InvalidName", "Courier name is required.", Shared.Errors.ErrorType.Validation));

            return new Courier(name, websiteUrl, trackingUrlTemplate, contactNumber);
        }

        // --- BEHAVIORS ---
        public void UpdateDetails(string name, string? websiteUrl, string? trackingUrlTemplate, string? phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            WebsiteUrl = websiteUrl;
            TrackingUrlTemplate = trackingUrlTemplate;
            PhoneNumber = phoneNumber;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
