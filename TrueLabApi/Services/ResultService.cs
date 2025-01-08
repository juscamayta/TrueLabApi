using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TruelabApi.Modelo;

namespace TruelabApi.Services
{
    public class ResultService : IResultService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://example.com/v1/"; // URL base de la API

        public ResultService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Implementación de StoreResults  // carga de resultados 
        public void StoreResults(UploadResultRequest resultRequest)
        {
            // se  puedes agregar la lógica para almacenar el resultado.
            
            // Ejemplo de simplemente imprimir los resultados (puedes sustituirlo con la lógica real)
            Console.WriteLine("Storing result...");
            Console.WriteLine($"Patient Name: {resultRequest.PatientName}");
            Console.WriteLine($"Age: {resultRequest.Age}");


        }

        // Implementación de UploadResultAsync
        public async Task UploadResultAsync(string token, UploadResultRequest result)
        {
            var resultUrl = $"{_baseUrl}results/upload";

            // Configurar el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Convertir el objeto UploadResultRequest a JSON
            var content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            // Enviar la solicitud POST
            var response = await _httpClient.PostAsync(resultUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Si la respuesta es exitosa, indicar que el resultado fue subido correctamente
                Console.WriteLine("Result uploaded successfully.");
            }
            else
            {
                // Si falla la solicitud, lanzar una excepción o manejar el error de otra manera
                throw new Exception("Failed to upload result.");
            }
        }
    }
}

