namespace TruelabApi.Modelo  // Define el espacio de nombres (namespace) donde se encuentra la clase LoginResponse.
{
    public class LoginResponse  // Define la clase LoginResponse. Esta clase se utiliza para representar la respuesta de una solicitud de inicio de sesión.
    {
        public string Message { get; set; }  // Propiedad pública para almacenar un mensaje de respuesta que el servidor puede devolver al cliente, por ejemplo, "Login successful".
        public string Token { get; set; }    // Propiedad pública para almacenar el token JWT que se genera tras un inicio de sesión exitoso.
        public bool Success { get; set; }    // Propiedad pública que indica si el inicio de sesión fue exitoso o no. 
    }
}
