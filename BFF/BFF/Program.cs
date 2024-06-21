using System.Text.Json.Serialization;
using BFF;
using BFF.Middleware;
using Dal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddSwaggerGen();

builder.Services.RegisterDbContexts(builder.Configuration);
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsPolicy",
        policy =>
        {
            policy.WithOrigins("https://localhost:4000");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (args.Contains("/seed"))
    {
        SeedData.EnsureSeedData(app);
        return;
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corsPolicy");
app.UseMiddleware<CsrfHeaderMiddleware>();

app.MapControllers();
app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.UseTokenMiddleware();
});

app.Run();