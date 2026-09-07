var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contentor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registar o HttpClient para consumo da API externa
builder.Services.AddHttpClient();

var app = builder.Build();

// Configurar a pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();