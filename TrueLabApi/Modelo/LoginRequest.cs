namespace TruelabApi.Modelo  // Define el espacio de nombres (namespace) donde se encuentra la clase LoginRequest.
{
    public class LoginRequest  // Define la clase LoginRequest. Esta clase se utiliza para representar la solicitud de inicio de sesión.
    {
        public string Username { get; set; }  // Propiedad pública para almacenar el nombre de usuario que el cliente envía en la solicitud de inicio de sesión.
        public string Password { get; set; }  // Propiedad pública para almacenar la contraseña que el cliente envía en la solicitud de inicio de sesión.
    }
}
