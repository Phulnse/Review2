using Serilog;
using WebAPI.Extensions;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.OpenCorsPolicy();
builder.Services.MakeDefaultDecisionOverdueJobConfigure();
builder.Services.SumarizeResultJobConfigure();
builder.Services.SendNotifyEmailJobConfigure();
builder.Services.AddAuthenticationConfigure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.DatabaseMigrate();

app.UseCors("open");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExeptionMiddleware>();

app.MapControllers();

app.Run();
