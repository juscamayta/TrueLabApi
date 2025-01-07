using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using TruelabApi.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly string _secretKey;

    // inyeccion de dependencias para agregar la clave en el appsettings //
    public AuthenticationService(IConfiguration configuration)
    {
        _secretKey = configuration.GetValue<string>("Jwt:SecretKey"); // Obtener la clave secreta de la configuración
    }

    // Método para autenticar al usuario y generar el token
    public string Authenticate(string username, string password) 
    {
        // Validación de las credenciales (en una implementación real, deberías validar contra una base de datos)
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new UnauthorizedAccessException("El nombre de usuario o la contraseña no pueden estar vacíos.");
        }

        // Simulación de autenticación; en una aplicación real, esto debería hacer una consulta a una base de datos
        if (username == "username" && password == "Password")  // Usa las credenciales proporcionadas
        {
            var tokenHandler = new JwtSecurityTokenHandler(); // de la libreria token para crear leer y validar 
            var key = Encoding.ASCII.GetBytes(_secretKey);  // Usamos la clave secreta desde la configuración y lo convertimos en bytes 

            // Creamos el descriptor del token , se usa para definir los detalles del token 
            var tokenDescriptor = new SecurityTokenDescriptor   
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)  // El nombre del usuario como un "claim"
                }),
                Expires = DateTime.UtcNow.AddHours(1),  // El token expira en 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)  // Firma el token con la clave secreta
            };

            try
            {
                // Crear el token
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);  // Retorna el token como string
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Error al generar token: " + ex.Message);
            }
        }

        // Si las credenciales son incorrectas, lanzamos una excepción
        throw new UnauthorizedAccessException("Credenciales no válidas");
    }

    // Método para validar el token JWT
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);  // Usamos la misma clave secreta para la validación

            // Configuración para validar el token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,  // Validar que el token no haya expirado
                IssuerSigningKey = new SymmetricSecurityKey(key)  // Clave secreta para validar la firma
            };

            // Validamos el token
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Si el token es válido, retornamos true
            return true;
        }
        catch (Exception)
        {
            // Si hay algún error (token inválido o expirado), retornamos false
            return false;
        }
    }
}
