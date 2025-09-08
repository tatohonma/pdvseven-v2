using a7D.PDV.Ativacao.API.Extensions;
using a7D.PDV.Ativacao.API.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Custom Extensions
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(builder.Configuration);

builder.Services.Configure<ErrorNotifyOptions>(builder.Configuration.GetSection("ErrorNotify"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorCaptureMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
