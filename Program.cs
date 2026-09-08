var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
            "http://127.0.0.1:5500",
            "http://localhost:5500"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("Frontend");

// Configuração do Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeGames API v1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();