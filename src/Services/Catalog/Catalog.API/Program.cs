using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container

builder.Services.AddCarter();

builder.Services.AddMediatR(configs =>
{
    configs.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    
    configs.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configs.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddExceptionHandler<CustomExcptionHandler>();

builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);


var app = builder.Build();


// Configure HTTP request pipeline

app.MapCarter();  // scan all that implement ICarterModule and map(expose) minimal APIs in application

app.UseExceptionHandler(options => { });

app.MapHealthChecks("/health",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
