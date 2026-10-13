using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vCore.Api.Authentication;
using vCore.Api.Security;
using vCore.Application.Services;
using vCore.Domain.Exceptions;
using vCore.Infrastructure.Authorization;
using vCore.Infrastructure.Persistence;
using vCore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<VCoreDbContext>(options => options.UseSqlServer(connectionString));
}
else
{
    builder.Services.AddDbContext<VCoreDbContext>(options => options.UseInMemoryDatabase("vCoreTenantDb"));
}

builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IAuthenticatedTenantUserContext, HttpContextAuthenticatedUserContext>();
builder.Services.AddScoped<ITenantApplicationService, vCore.Application.Services.TenantApplicationService>();
var configuredStatus = builder.Configuration["Tenant:InitialStatus"];
builder.Services.AddSingleton<ITenantRegistrationStatusPolicy>(new DefaultTenantRegistrationStatusPolicy(configuredStatus));

builder.Services.AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, StaticAuthenticationHandler>("Bearer", _ => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(TenantPolicies.TenantRegistration, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantProfileManagement, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantStatusManagement, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantProvisioning, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantHierarchyManagement, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantDomainManagement, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantLifecycleHistory, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(TenantPolicies.TenantProvisioningHistory, policy => policy.RequireAuthenticatedUser());
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var problem = new ProblemDetails
        {
            Title = "An error occurred while processing the request.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception?.Message ?? "Unexpected server error.",
            Instance = context.Request.Path
        };

        if (exception is TenantValidationException)
        {
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Title = "Validation error";
        }
        else if (exception is TenantCodeAlreadyExistsException)
        {
            problem.Status = StatusCodes.Status409Conflict;
            problem.Title = "Tenant code already exists";
        }
        else if (exception is TenantConcurrencyException)
        {
            problem.Status = StatusCodes.Status409Conflict;
            problem.Title = "Concurrency conflict";
        }
        else if (exception is DbUpdateConcurrencyException)
        {
            problem.Status = StatusCodes.Status409Conflict;
            problem.Title = "Concurrency conflict";
        }
        else if (exception is TenantNotFoundException)
        {
            problem.Status = StatusCodes.Status404NotFound;
            problem.Title = "Tenant not found";
        }
        else if (exception is TenantAccessDeniedException)
        {
            problem.Status = StatusCodes.Status403Forbidden;
            problem.Title = "Access denied";
        }

        context.Response.StatusCode = problem.Status.Value;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;

