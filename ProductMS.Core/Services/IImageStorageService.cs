using System.Threading.Tasks;

namespace ProductMS.Core.Services
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(string tempUrl, string productId);
        Task DeleteImageAsync(string imageUrl);
    }
}