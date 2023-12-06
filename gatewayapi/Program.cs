using gatewayapi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<JwtAuthMiddleware>();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();