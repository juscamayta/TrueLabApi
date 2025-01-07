namespace TruelabApi.Services  // Define el espacio de nombres (namespace) donde se encuentra la interfaz IAuthenticationService.
{
    public interface IAuthenticationService  // Define la interfaz IAuthenticationService, que representa los métodos que debe implementar un servicio de autenticación.
    {
        // Método de autenticación que debe ser implementado por las clases que implementen esta interfaz.
        // Recibe el nombre de usuario y la contraseña y devuelve un token de autenticación.
        string Authenticate(string username, string password);
        bool ValidateToken(string token);
    }
}
