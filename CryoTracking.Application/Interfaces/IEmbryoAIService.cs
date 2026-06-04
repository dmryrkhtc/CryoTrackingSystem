using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface IEmbryoAIService
    {
        Task<double?> GetAIScoreAsync(IFormFile embryoImage);
    }
}