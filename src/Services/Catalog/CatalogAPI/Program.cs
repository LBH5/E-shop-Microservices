


var builder = WebApplication.CreateBuilder(args);


// Add the services to DI container
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDbConnection")!);
})
 .UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddCarter(); 

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogDbConnection")!);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler(options => { });

app.MapCarter();
app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
