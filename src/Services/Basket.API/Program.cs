
using Discount.gRPC;

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

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(handlers =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = 
    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

builder.Services.AddExceptionHandler<CustomExcptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetConnectionString("Redis");
});


builder.Services.AddScoped<IBasketRepository,  BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapCarter();  // scan all that implement ICarterModule and map(expose) minimal APIs in application

app.UseExceptionHandler(options => { });

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();
