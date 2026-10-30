
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // tell Marten to use UserName as the identity for ShoppingCart

}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository,  BasketRepository>();

builder.Services.AddExceptionHandler<CustomExcptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database"));


var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapCarter();  // scan all that implement ICarterModule and map(expose) minimal APIs in application

app.UseExceptionHandler(options => { });

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();
