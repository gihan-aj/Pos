using FluentValidation;
using JasperFx.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pos.Web.Infrastructure;
using Pos.Web.Infrastructure.Middleware;
using Pos.Web.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// 1. Setup Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddSingleton<AuditingInterceptor>();
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString);
    options.AddInterceptors(sp.GetRequiredService<AuditingInterceptor>());
}, optionsLifetime: ServiceLifetime.Singleton);

// 2. Setup Wolverine
builder.Host.UseWolverine(opts =>
{
    //opts.PersistMessagesWithSqlServer(connectionString);

    // Use the outbox pattern with EF Core for robust messaging
    //opts.UseEntityFrameworkCoreTransactions();
    //opts.Policies.Add<ResultTransactionalPolicy>();
    //opts.Policies.AutoApplyTransactions();

    opts.UseFluentValidation();

    // Auto-discover handlers in the assembly
    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);
});
builder.Services.AddWolverineHttp();

// 3. Setup Validation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// 4. Setup Error Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 5. Auth (Connect to your OpenIddict provider)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = "[http://localhost:8080](http://localhost:8080)";
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

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });

    app.MapScalarApiReference();
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

// 6. Map Wolverine Endpoints (VSA Style)
app.MapWolverineEndpoints(opts =>
{
    opts.UseFluentValidationProblemDetailMiddleware();

});

app.Run();