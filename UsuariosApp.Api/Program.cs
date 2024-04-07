using UsuariosApp.Api.Extensions;
using UsuariosApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSwaggerDoc();
builder.Services.AddIdentityContext(builder.Configuration);
builder.Services.AddJwtBearer(builder.Configuration);
builder.Services.AddCorsConfig();

var app = builder.Build();

app.UseSwaggerDoc();
app.UseCorsConfig();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }