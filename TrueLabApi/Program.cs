using Microsoft.AspNetCore.Authentication.JwtBearer;  // Importa el espacio de nombres necesario para JWT.
using Microsoft.IdentityModel.Tokens;  // Para la validaci�n del token.
using TruelabApi.Services;  // Espacio de nombres del servicio de autenticaci�n.
using System.Text;  
using Microsoft.Extensions.Configuration; 
using Microsoft.AspNetCore.Mvc;  // Para controladores de la API.
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);  // Crea el builder para configurar la aplicaci�n web.

// se agrega servicios necesarios para la aplicaci�n web
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


// Configura la autenticaci�n JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;  // Establecer en true para producci�n (recomendado).
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,  // Puedes habilitar en producci�n si tienes un emisor.
            ValidateAudience = false,  // Puedes habilitar en producci�n si tienes una audiencia espec�fica.
            ValidateLifetime = true,  // Valida que el token no haya expirado.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Usar la clave secreta desde el archivo de configuraci�n.
        };
    });


//  CORS para permitir todas las solicitudes (ajustar para producci�n seg�n sea necesario).
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Registra el servicio de autenticaci�n para inyecci�n de dependencias
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Registra el servicio de resultados para inyecci�n de dependencias
builder.Services.AddScoped<IResultService, ResultService>();  // Aqu� registramos IResultService con su implementaci�n ResultService

// Configura el cliente HTTP para la comunicaci�n con otros servicios (si es necesario)
builder.Services.AddHttpClient("TruelabApi", client =>
{
    client.BaseAddress = new Uri("https://example.com/v1/");  // Aqu� coloca la URL base de la API externa si es necesario.
});

var app = builder.Build();  // Construye la aplicaci�n web.

app.UseSwagger();  // Habilita Swagger para la documentaci�n de la API.
app.UseSwaggerUI();  // Habilita la interfaz de usuario de Swagger para interactuar con la API.

app.UseHttpsRedirection();  // Redirige todas las solicitudes HTTP a HTTPS.
app.UseAuthentication();  // Habilita el middleware de autenticaci�n para validar los JWT.
app.UseAuthorization();  // Habilita el middleware de autorizaci�n para proteger rutas.

app.UseCors("AllowAll");  // Permite todas las solicitudes CORS (ajustar seg�n sea necesario para producci�n).

app.MapControllers();  // Mapea los controladores de la API para las rutas configuradas.

app.Run();  // Inicia el servidor web.
