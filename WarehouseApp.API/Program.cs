using Microsoft.EntityFrameworkCore;
using WarehouseApp.Persistence;
using WarehouseApp.Application.Abstractions.Date;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Infrastructure;
using MediatR;
using WarehouseApp.Application.Companies.Create;
using WarehouseApp.Application.Companies.Update;
using WarehouseApp.Application.Companies.Delete;
using WarehouseApp.Application.Companies.GetAll;
using WarehouseApp.Application.Companies.GetById;
using WarehouseApp.Application.Companies.GetActive;
using WarehouseApp.SharedKernel.Core.Primitives.Results;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IDateTime, DateTimeProvider>();
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

// Add MediatR (after registering dependencies)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCompanyCommand).Assembly));

var app = builder.Build();

// Debug: Log registered services
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider.GetServices<IRequestHandler<CreateCompanyCommand, Result<Guid>>>();
    Console.WriteLine($"Found {services.Count()} handlers for CreateCompanyCommand");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Basic health endpoints
app.MapGet("/", () => "Warehouse API is running!");
app.MapGet("/health", () => "OK");
app.MapGet("/api/status", () => new { Status = "Running", Timestamp = DateTime.UtcNow });

// Company endpoints using CQRS pattern
app.MapGet("/api/companies", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetAllCompaniesQuery());
    
    return result.IsSuccess 
        ? Results.Ok(result.Value) 
        : Results.BadRequest(new { error = result.Error.Message });
})
.WithName("GetAllCompanies")
.WithOpenApi();

app.MapGet("/api/companies/active", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetActiveCompaniesQuery());
    
    return result.IsSuccess 
        ? Results.Ok(result.Value) 
        : Results.BadRequest(new { error = result.Error.Message });
})
.WithName("GetActiveCompanies")
.WithOpenApi();

app.MapGet("/api/companies/{id:guid}", async (Guid id, IMediator mediator) =>
{
    var result = await mediator.Send(new GetCompanyByIdQuery(id));
    
    return result.IsSuccess 
        ? Results.Ok(result.Value) 
        : Results.NotFound(new { error = result.Error.Message });
})
.WithName("GetCompanyById")
.WithOpenApi();

app.MapPost("/api/companies", async (CreateCompanyCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    
    return result.IsSuccess 
        ? Results.CreatedAtRoute("GetCompanyById", new { id = result.Value }, new { id = result.Value }) 
        : Results.BadRequest(new { error = result.Error.Message });
})
.WithName("CreateCompany")
.WithOpenApi();

app.MapPut("/api/companies/{id:guid}", async (Guid id, UpdateCompanyCommand command, IMediator mediator) =>
{
    var updateCommand = command with { Id = id };
    var result = await mediator.Send(updateCommand);
    
    return result.IsSuccess 
        ? Results.NoContent() 
        : Results.BadRequest(new { error = result.Error.Message });
})
.WithName("UpdateCompany")
.WithOpenApi();

app.MapDelete("/api/companies/{id:guid}", async (Guid id, IMediator mediator) =>
{
    var result = await mediator.Send(new DeleteCompanyCommand(id));
    
    return result.IsSuccess 
        ? Results.NoContent() 
        : Results.BadRequest(new { error = result.Error.Message });
})
.WithName("DeleteCompany")
.WithOpenApi();

app.Run();