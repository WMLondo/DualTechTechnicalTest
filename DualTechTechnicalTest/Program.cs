using DualTechTechnicalTest.Domain;
using DualTechTechnicalTest.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDomain(builder.Configuration);
builder.Services.AddAutoMapper(options => { },AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseExceptionHandler("/Error");

app.Run();