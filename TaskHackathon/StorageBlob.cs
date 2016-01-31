using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TaskHackathon
{
    public class StorageBlob
    {

        private CloudBlobClient blobClient;

        CloudStorageAccount storageAccount = null;

        private string containerName;

        public StorageBlob(string connectionString, string containerName)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            
            blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            
            //storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            //storageAccount = CloudStorageAccount.Parse(connectionString);
            //storageAccount.CreateCloudBlobClient();
            //blobClient = storageAccount.CreateCloudBlobClient();
            //containerName = String.Format("{0}{1}", containername, DefaultConfiguration.AzureDeploymentId);
            this.containerName = containerName;
        }

        /// <summary>
        /// Function to get Blob container
        /// </summary>
        /// <param name="containerName">container to look</param>
        /// <returns>Blob container</returns>
        public CloudBlobContainer GetBlobContainer()
        {
            // get the container reference
            var blobContainer = blobClient.GetContainerReference(containerName);

            try
            {
                // Create the container if it does not exist.
                var options = new BlobRequestOptions
                {
                    MaximumExecutionTime = TimeSpan.FromSeconds(2),
                };
                if (blobContainer.CreateIfNotExists())
                {
                    // Set permissions on the container, if it was created.
                    var containerPermissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Off
                    };
                    blobContainer.SetPermissions(containerPermissions);
                }
            }
            catch (Exception ex)
            {
            }

            return blobContainer;
        }

        /// <summary>
        /// downloads the specified blob as string
        /// </summary>
        /// <param name="blobName">blob name</param>
        /// <param name="fileContent">file content string</param>
        public string DownloadFileAsText(string blobName)
        {
            string fileContent = string.Empty;
            var blobContainer = GetBlobContainer();
            // download the blob at downloadDirectoryPath
            try
            {
                var blob = blobContainer.GetBlockBlobReference(blobName);
                fileContent = blob.DownloadText();
            }
            catch (Exception ex)
            {
            }
            return fileContent;
        }

        /// <summary>
        /// downloads the specified blob as string
        /// </summary>
        /// <param name="blobName">blob name</param>
        public async Task<string> DownloadFileAsTextAsync(string blobName)
        {
            string fileContent = string.Empty;
            var blobContainer = GetBlobContainer();
            // download the blob at downloadDirectoryPath
            try
            {
                var blob = blobContainer.GetBlockBlobReference(blobName);
                fileContent = await blob.DownloadTextAsync();
            }
            catch (Exception ex)
            {
            }
            return fileContent;
        }


        /// <summary>
        /// uploads the specified string as blob
        /// </summary>
        /// <param name="blobName">blob name</param>
        /// <param name="data">file content string</param>
        public void UploadFile(string blobName, string data)
        {
            var blobContainer = GetBlobContainer();
            var blob = blobContainer.GetBlockBlobReference(blobName);

            blob.UploadText(data);

            /***** TO BE DELETED LATER**************/
            //int id = 0;
            //int byteslength = data.Length;
            //int bytesread = 0;
            //int index = 0;
            //List<string> blocklist = new List<string>();
            //int numBytesPerChunk = 250 * 1024; //250KB per block
            //do
            //{
            //    byte[] buffer = new byte[numBytesPerChunk];
            //    int limit = index + numBytesPerChunk;
            //    for (int loops = 0; index < limit; index++)
            //    {
            //        buffer[loops] = data[index];
            //        loops++;
            //    }
            //    bytesread = index;
            //    string blockIdBase64 = Convert.ToBase64String(System.BitConverter.GetBytes(id));

            //    blob.PutBlock(blockIdBase64, new MemoryStream(buffer, true), null);
            //    blocklist.Add(blockIdBase64);
            //    id++;
            //} while (byteslength - bytesread > numBytesPerChunk);

            //int final = byteslength - bytesread;
            //byte[] finalbuffer = new byte[final];
            //for (int loops = 0; index < byteslength; index++)
            //{
            //    finalbuffer[loops] = data[index];
            //    loops++;
            //}
            //string blockId = Convert.ToBase64String(System.BitConverter.GetBytes(id));
            //blob.PutBlock(blockId, new MemoryStream(finalbuffer, true), null);
            //blocklist.Add(blockId);
            //blob.PutBlockList(blocklist);
        }

        public void UploadZipFromFile(string blobName, string filePath)
        {
            var blobContainer = GetBlobContainer();

            try
            {
                using (var compressed = new FileStream(filePath, FileMode.Open))
                {
                    var blob = blobContainer.GetBlockBlobReference(blobName);
                    blob.Properties.ContentEncoding = "zip";
                    blob.UploadFromStream(compressed);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Dictionary<string, string> DownLoadAndExtract(string blobName)
        {
            var blobContainer = GetBlobContainer();
            var contents = new Dictionary<string, string>();

            try
            {
                var blob = blobContainer.GetBlockBlobReference(blobName);

                if (blob.Exists())
                {
                    using (var compressed = new MemoryStream())
                    {
                        blob.DownloadToStream(compressed);

                        using (ZipArchive archive = new ZipArchive(compressed))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                using (var fileCompressed = entry.Open())
                                {
                                    using (var reader = new StreamReader(fileCompressed))
                                    {
                                        contents.Add(Path.GetFileNameWithoutExtension(entry.Name), reader.ReadToEnd());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return contents;
        }

        public void CompressAndUpload(string blobName, string data)
        {
            var blobContainer = GetBlobContainer();

            try
            {
                using (var compressed = new MemoryStream())
                {
                    using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
                    {
                        var bytes = Encoding.UTF8.GetBytes(data);
                        var blob = blobContainer.GetBlockBlobReference(blobName);
                        gzip.Write(bytes, 0, bytes.Length);
                        blob.Properties.ContentEncoding = "gzip";

                        compressed.Position = 0;

                        blob.UploadFromStream(compressed);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string DownloadAndExtract(string blobName, int approximateFileSize)
        {
            var blobContainer = GetBlobContainer();
            var blobContent = string.Empty;

            try
            {
                using (var compressed = new MemoryStream())
                {
                    var blob = blobContainer.GetBlockBlobReference(blobName);

                    if (blob.Exists())
                    {
                        blob.DownloadToStream(compressed);
                        compressed.Position = 0;

                        if (compressed.Length <= 0)
                        {
                            return blobContent;
                        }

                        byte[] buffer = new byte[approximateFileSize];

                        using (var gzip = new GZipStream(compressed, CompressionMode.Decompress))
                        {
                            gzip.Read(buffer, 0, buffer.Length);
                        }

                        blobContent = Encoding.UTF8.GetString(buffer);
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return blobContent;
        }
    }
}
