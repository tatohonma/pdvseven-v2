using a7D.PDV.Ativacao.API.Extensions;
using a7D.PDV.Ativacao.API.Options;
using Cariatides.Data.Seeders;
using Cariatides.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Custom Extensions
builder.Services.AddSwaggerGen();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddApplicationServices();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCustomIdentity(builder.Configuration, builder.Environment);
builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:1654")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});



builder.Services.Configure<ErrorNotifyOptions>(builder.Configuration.GetSection("ErrorNotify"));

var app = builder.Build();


if (args.Contains("--seed"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await SeederManager.Run(services);
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("teste");
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("FrontendDev");
app.UseMiddleware<ErrorCaptureMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
