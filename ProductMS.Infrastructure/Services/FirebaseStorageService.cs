using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using ProductMS.Core.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductMS.Infrastructure.Services
{
    public class FirebaseStorageService : IImageStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseStorageService(StorageClient storageClient, IOptions<FirebaseSettings> settings)
        {
            _storageClient = storageClient;
            _bucketName = settings.Value.Bucket;
        }

        public async Task<string> UploadImageAsync(string tempUrl, string productId)
        {
            using var httpClient = new HttpClient();
            var imageStream = await httpClient.GetStreamAsync(tempUrl);
            var fileName = $"products/{productId}/{Guid.NewGuid()}.jpg";

            await _storageClient.UploadObjectAsync(
                bucket: _bucketName,
                objectName: fileName,
                contentType: "image/jpeg",
                source: imageStream,
                options: new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead
                });

            return $"https://storage.googleapis.com/{_bucketName}/{fileName}";
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var fileName = uri.AbsolutePath.TrimStart('/');
            await _storageClient.DeleteObjectAsync(_bucketName, fileName);
        }
    }
}