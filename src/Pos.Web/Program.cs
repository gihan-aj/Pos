using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pos.Web.Features.Catalog.Categories;
using Pos.Web.Features.Catalog.Products;
using Pos.Web.Features.Couriers;
using Pos.Web.Features.Customers;
using Pos.Web.Features.Orders;
using Pos.Web.Infrastructure;
using Pos.Web.Infrastructure.Behaviors;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Infrastructure.Services;
using Pos.Web.Shared.Abstractions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Setup Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddSingleton<AuditingInterceptor>();
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString);
    options.AddInterceptors(sp.GetRequiredService<AuditingInterceptor>());
});

builder.Services.AddScoped<IAppSequenceService, AppSequenceService>();

// 1. Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    // Register the Validation Behavior
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// 3. Setup Validation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// 4. Setup Error Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 5. Auth (Connect to your OpenIddict provider)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080";
        options.Audience = "pos-api";

        // For development, allow http if needed (though you are using https)
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // OpenIddict sets the "aud" (audience) claim to the resource server IDs.
            // Since we haven't registered resources yet, we disable this check for now.
            ValidateAudience = true,
            ValidAudience = "pos-api",

            // OpenIddict strictly sets the token type to "at+jwt".
            // We must tell the validator to accept this specific type.
            ValidTypes = new[] { "at+jwt" }
        };
    });
builder.Services.AddAuthorization();

// 6. Add Open API
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("POS API")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure DB exists (optional, good for local dev)
    dbContext.Database.EnsureCreated();

    // Run Seeder
    var seeder = new Pos.Web.Infrastructure.Persistence.Seed.DataSeeder(dbContext);
    await seeder.SeedAsync();
}

app.MapCategoryEndpoints();
app.MapProductEndpoints();
app.MapCustomerEndpoints();
app.MapOrderEndpoints();
app.MapCourierEndpoints();

app.Run();