using System.Threading.Tasks;
using TruelabApi.Modelo;

namespace TruelabApi.Services
{
    public interface IResultService
    {
        // Método que almacena los resultados proporcionados en la solicitud.
        void StoreResults(UploadResultRequest resultRequest);

        // Método asincrónico que sube los resultados de manera asíncrona usando un token de autenticación.
        Task UploadResultAsync(string token, UploadResultRequest result);
    }
}
