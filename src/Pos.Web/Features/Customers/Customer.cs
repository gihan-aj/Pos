using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Customers
{
    public sealed class Customer : AuditableEntity
    {
        private Customer() { }

        private Customer(
            string name,
            string phoneNumber,
            string? email,
            string? address,
            string? city,
            string? country,
            string? postalCode,
            string? region,
            string? notes)
        {
            Id = Guid.NewGuid();
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            City = city;
            Country = country;
            PostalCode = postalCode;
            Region = region;
            Notes = notes;
            IsActive = true;
        }

        public string Name { get; private set; } = string.Empty;

        public string PhoneNumber { get; private set; } = string.Empty;

        public string? Email { get; private set; }

        public string? Address { get; private set; }

        public string? City { get; private set; }

        public string? Country { get; private set; }

        public string? PostalCode { get; private set; }

        public string? Region { get; private set; }

        public string? Notes { get; private set; }

        public bool IsActive { get; private set; } = true;

        public static Result<Customer> Create(
            string name,
            string phoneNumber,
            string? email = null,
            string? address = null,
            string? city = null,
            string? country = null,
            string? postalCode = null,
            string? region = null,
            string? notes = null)
        {
            if(string.IsNullOrWhiteSpace(name))
                return Result<Customer>.Failure(new Error("Customer.NameEmpty","Customer name cannot be empty.", ErrorType.Validation));
            
            if(string.IsNullOrWhiteSpace(phoneNumber))
                return Result<Customer>.Failure(new Error("Customer.PhoneNumberEmpty","Customer phone number cannot be empty.", ErrorType.Validation));
            
            return new Customer(
                name,
                phoneNumber,
                email,
                address,
                city,
                country,
                postalCode,
                region,
                notes);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public Result UpdateDetails(
            string name,
            string phoneNumber,
            string? email = null,
            string? address = null,
            string? city = null,
            string? country = null,
            string? postalCode = null,
            string? region = null,
            string? notes = null)
        {
            if(!string.IsNullOrWhiteSpace(name))
                Name = name;
            
            if(!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;
            
            Email = email;
            Address = address;
            City = city;
            Country = country;
            PostalCode = postalCode;
            Region = region;
            Notes = notes;

            return Result.Success();
        }
    }
}
