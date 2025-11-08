using CSRF_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<LedgerService>();

// Configurar CORS para permitir requests del frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8000", 
                "http://127.0.0.1:8000", 
                "http://localhost:5500",
                "http://localhost:3000",
                "http://127.0.0.1:3000",
                "null" // Para file://
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Necesario para cookies
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configurar para minimizar schemas mostrados
    options.CustomSchemaIds(type => type.Name);
    options.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1); // Ocultar modelos/schemas por defecto
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}

app.UseHttpsRedirection();

// Usar CORS antes de Authorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Configurar puerto 5029 para que coincida con el script de inicio
app.Urls.Add("http://localhost:5029");

app.Run();
