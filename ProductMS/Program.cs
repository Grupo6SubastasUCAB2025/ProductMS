using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ProductMS.Application.Commands;
using ProductMS.Core.EventBus;
using ProductMS.Core.Persistence.Repositories.Mongo;
using ProductMS.Core.Persistence.Repositories.PostgreSQL;
using ProductMS.Core.Services;
using ProductMS.Infrastructure.Contexts;
using ProductMS.Infrastructure.EventBus;
using ProductMS.Infrastructure.EventBus.Consumers;
using ProductMS.Infrastructure.Persistence.Repositories.Mongo;
using ProductMS.Infrastructure.Persistence.Repositories.PostgreSQL;
using ProductMS.Infrastructure.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración base del API
builder.Services.AddControllers();

// 2. Configuración de MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// 3. Configuración de FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);

// 4. Configuración de PostgreSQL SIN MIGRACIONES
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// 5. Configuración de MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));
builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase("productos_db"));

// 6. Configuración de RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["ConnectionStrings:RabbitMQ"]));
        cfg.ReceiveEndpoint("product-created", e =>
        {
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
        });
    });
});

// 7. Configuración de Firebase
var firebaseCredentialsFileName = builder.Configuration["FirebaseSettings:CredentialsFileName"] ?? "firebase-credentials.json";
var isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var firebaseCredentialsPath = isRunningInDocker
    ? $"/app/{firebaseCredentialsFileName}" // Ruta en el contenedor
    : Path.Combine(Directory.GetCurrentDirectory(), "ProductMS.Infrastructure", firebaseCredentialsFileName); // Ruta local

if (!File.Exists(firebaseCredentialsPath))
{
    throw new FileNotFoundException($"Archivo de credenciales de Firebase no encontrado en: {firebaseCredentialsPath}", firebaseCredentialsPath);
}

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
});

// Registros explícitos de servicios
builder.Services.AddSingleton(StorageClient.Create(GoogleCredential.FromFile(firebaseCredentialsPath)));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IMongoProductRepository, MongoProductRepository>();
builder.Services.AddScoped<IImageStorageService, FirebaseStorageService>();
builder.Services.AddScoped<IEventBus, RabbitEventBus>();
builder.Services.AddScoped<ProductCreatedConsumer>();

// 8. Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 9. Configuración del pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 10. VERIFICACIÓN SIMPLE DE CONEXIÓN (sin migraciones)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    try
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        if (!canConnect)
        {
            throw new Exception("No se pudo conectar a PostgreSQL. Verifica la cadena de conexión y que el servicio de PostgreSQL esté funcionando.");
        }
        Console.WriteLine("Conexión a PostgreSQL exitosa.");
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error de conexión a PostgreSQL");
        logger.LogError("Verifica que:");
        logger.LogError("1. El servicio 'db_postgres' esté corriendo en Docker Compose.");
        logger.LogError("2. La base de datos 'productos_db' exista.");
        logger.LogError("3. El usuario 'dockeruser' y la contraseña sean correctos.");
        logger.LogError("4. La aplicación tenga permisos para acceder a la base de datos.");
    }
}

app.Run();