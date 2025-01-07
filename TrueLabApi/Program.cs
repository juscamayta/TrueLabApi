using Microsoft.AspNetCore.Authentication.JwtBearer;  // Importa el espacio de nombres necesario para JWT.
using Microsoft.IdentityModel.Tokens;  // Para la validación del token.
using TruelabApi.Services;  // Espacio de nombres del servicio de autenticación.
using System.Text;  
using Microsoft.Extensions.Configuration; 
using Microsoft.AspNetCore.Mvc;  // Para controladores de la API.
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);  // Crea el builder para configurar la aplicación web.

// se agrega servicios necesarios para la aplicación web
builder.Services.AddControllers();  // Registra los controladores de la API.
builder.Services.AddEndpointsApiExplorer();  // Registra el servicio para explorar los endpoints de la API.
builder.Services.AddSwaggerGen(c =>
{
    // Configurar la seguridad para Swagger (Bearer Token)
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});  


// Configura la autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;  // Establecer en true para producción (recomendado).
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,  // Puedes habilitar en producción si tienes un emisor.
            ValidateAudience = false,  // Puedes habilitar en producción si tienes una audiencia específica.
            ValidateLifetime = true,  // Valida que el token no haya expirado.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Usar la clave secreta desde el archivo de configuración.
        };
    });


//  CORS para permitir todas las solicitudes (ajustar para producción según sea necesario).
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Registra el servicio de autenticación para inyección de dependencias
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Registra el servicio de resultados para inyección de dependencias
builder.Services.AddScoped<IResultService, ResultService>();  // Aquí registramos IResultService con su implementación ResultService

// Configura el cliente HTTP para la comunicación con otros servicios (si es necesario)
builder.Services.AddHttpClient("TruelabApi", client =>
{
    client.BaseAddress = new Uri("https://example.com/v1/");  // Aquí coloca la URL base de la API externa si es necesario.
});

var app = builder.Build();  // Construye la aplicación web.

app.UseSwagger();  // Habilita Swagger para la documentación de la API.
app.UseSwaggerUI();  // Habilita la interfaz de usuario de Swagger para interactuar con la API.

app.UseHttpsRedirection();  // Redirige todas las solicitudes HTTP a HTTPS.
app.UseAuthentication();  // Habilita el middleware de autenticación para validar los JWT.
app.UseAuthorization();  // Habilita el middleware de autorización para proteger rutas.

app.UseCors("AllowAll");  // Permite todas las solicitudes CORS (ajustar según sea necesario para producción).

app.MapControllers();  // Mapea los controladores de la API para las rutas configuradas.

app.Run();  // Inicia el servidor web.
