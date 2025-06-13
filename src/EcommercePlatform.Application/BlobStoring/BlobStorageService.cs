using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Content;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.BlobStoring;

public class BlobStorageService : DomainService, IBlobStorageService
{
    private readonly IBlobContainer _container;
    private readonly IConfiguration _configuration;
    private readonly DefaultBlobContainerConfigurationProvider _blobContainerConfigurationProvider;
    private readonly DefaultBlobFilePathCalculator _defaultBlobFilePathCalculator;

    public BlobStorageService(IBlobContainer container, IConfiguration configuration, DefaultBlobContainerConfigurationProvider blobContainerConfigurationProvider, DefaultBlobFilePathCalculator defaultBlobFilePathCalculator)
    {
        _container = container;
        _configuration = configuration;
        _blobContainerConfigurationProvider = blobContainerConfigurationProvider;
        _defaultBlobFilePathCalculator = defaultBlobFilePathCalculator;
    }

    public async Task<string> SaveImageAsync(IRemoteStreamContent image, string containerName = "ecommerce")
    {
        using (CurrentTenant.Change(null))
        {
            if (image == null)
            {
                throw new UserFriendlyException("Image cannot be null");
            }

            string imageName = GuidGenerator.Create() + Regex.Replace(image.FileName, @"\s+", string.Empty);
            await _container.SaveAsync(imageName, image.GetStream());
            return imageName;
        }
    }

    public async Task<IRemoteStreamContent> GetImageAsync(string name, string containerName = "ecommerce")
    {
        using (CurrentTenant.Change(null))
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var blob = await _container.GetAsync(name);
            if (blob == null)
            {
                return null;
            }

            return new RemoteStreamContent(blob, name);
        }
    }

    public async Task DeleteImageAsync(string name, string containerName = "ecommerce")
    {
        using (CurrentTenant.Change(null))
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            await _container.DeleteAsync(name);
        }
    }

    public string GetImageUrl(string name, string containerName = "ecommerce")
    {
        using (CurrentTenant.Change(null))
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var containerConfig = _blobContainerConfigurationProvider.Get(containerName);
            var physicalPath = _defaultBlobFilePathCalculator.Calculate(new BlobProviderGetArgs(containerName, containerConfig, name));
            
            int wwwrootIndex = physicalPath.IndexOf("wwwroot", StringComparison.OrdinalIgnoreCase);
            if (wwwrootIndex == -1)
            {
                return null;
            }

            string relativePath = physicalPath.Substring(wwwrootIndex + "wwwroot".Length);
            return $"{_configuration.GetSection("App")["SelfUrl"]}{relativePath}";
        }
    }
} 