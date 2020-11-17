using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DineOut.Common
{
    public class AzureConnection
    {
        const string accountName = "dineout5";
        const string accountKey = "dbfbz68bPqcki2XygjEnn1SYZPNb4bg/g4vXXvJDxdmJbEiAlZPHQrW2DWHiX7gi5/abRojfscnARnEAb+j7dw==";
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient blobClient;
        private readonly CloudBlobContainer container;

        public AzureConnection()
        {
            storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("images");

            container.CreateIfNotExistsAsync();
            container.SetPermissionsAsync(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        public void UploadImageMemoryStream(string fileName, MemoryStream memoryStream)
        {
            var blob = container.GetBlockBlobReference(fileName);

            var task = blob.UploadFromStreamAsync(memoryStream);

            task.Wait();

        }
    }
}
