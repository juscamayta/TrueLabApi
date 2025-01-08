using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;  // Importa el espacio de nombres que contiene las clases necesarias para crear controladores y manejar solicitudes HTTP en ASP.NET Core.
using TruelabApi.Modelo;          
using TruelabApi.Services;        

namespace TruelabApi.Controllers   
{
    
    [Route("v1/devices")]          
    [ApiController]                
    public class DevicesController : ControllerBase  // Define un controlador que hereda de ControllerBase (en lugar de Controller), que es adecuado para APIs sin vistas.
    {
         
        private readonly IAuthenticationService _authenticationService;
        private readonly IResultService _resultService;

        
        public DevicesController(IAuthenticationService authenticationService, IResultService resultService)
        {
            _authenticationService = authenticationService;  
            _resultService = resultService;
        }

       // "/v1/devices/login"
        [HttpPost("login")]  // Indica que esta acción se activa cuando se realiza una solicitud HTTP POST a "/v1/devices/login".
        public IActionResult Login([FromBody] LoginRequest loginRequest)  // El parámetro "loginRequest" es deserializado automáticamente desde el cuerpo de la solicitud HTTP.
        {
            // Llama al método Authenticate del servicio de autenticación para validar el nombre de usuario y la contraseña.
            var token = _authenticationService.Authenticate(loginRequest.Username, loginRequest.Password);

            // Si el token es nulo, significa que las credenciales no son válidas.
            if (token == null)
            {
                // Devuelve una respuesta 401 Unauthorized con un mensaje de error.
                return Unauthorized(new { message = "Credenciales no válidas" });
            }

            // Si las credenciales son válidas, devuelve una respuesta 200 OK con un objeto de LoginResponse.
            return Ok(new LoginResponse     
            {
                Message = loginRequest.Username,  // El mensaje contiene el nombre de usuario.
                Token = "Bearer " + token,                    // El token de a  utenticación se incluye en la respuesta.
                Success = true                    // Indica que la autenticación fue exitosa.
            });
        }

        // Acción para el endpoint "/v1/devices/results/upload"
        [HttpPost("results/upload")]  // Indica que esta acción se activa cuando se realiza una solicitud HTTP POST a "/v1/devices/results/upload".
        [Authorize]  // Solo los usuarios autenticados pueden acceder a este endpoint.
        public IActionResult UploadResults([FromBody] UploadResultRequest resultRequest)
        {

            // Recuperar el token de la cabecera de autorización
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token no proporcionado" });
            }

            try
            {
                // Almacenar los resultados
                //_resultService.StoreResults(resultRequest);

                _resultService.UploadResultAsync(token, resultRequest);

                // Responder con un mensaje de éxito
                return Ok(new { message = "Resultados cargados exitosamente" });
            }
            catch (Exception ex)
            {
                // En caso de error, responder con un código de error 500
                return StatusCode(500, new { message = "Se produjo un error al cargar los resultados", error = ex.Message });
            }
        }
    }
}
