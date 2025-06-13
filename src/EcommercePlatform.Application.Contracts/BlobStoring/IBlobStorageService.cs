using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace EcommercePlatform.BlobStoring;

public interface IBlobStorageService : IApplicationService
{
    Task<string> SaveImageAsync(IRemoteStreamContent image, string containerName = "ecommerce");
    Task<IRemoteStreamContent> GetImageAsync(string name, string containerName = "ecommerce");
    Task DeleteImageAsync(string name, string containerName = "ecommerce");
    string GetImageUrl(string name, string containerName = "ecommerce");
} 