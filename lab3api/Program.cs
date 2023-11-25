var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();
//app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();