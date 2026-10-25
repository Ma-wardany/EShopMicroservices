

using Carter;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container

builder.Services.AddCarter();

builder.Services.AddMediatR(configs =>
{
    configs.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();


var app = builder.Build();

// Configure HTTP request pipeline

app.MapCarter();  // scan all that implement ICarterModule and map(expose) minimal APIs in application


app.Run();
