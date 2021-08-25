using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Util.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Basic.Services.External
{
    public interface IAzureStorage
    {
        Pageable<BlobItem> GetAll();
        BlobClient Get(string nome);
        BlobClient Get(TransaferenceFile file);
        Response<BlobContentInfo> Upload(TransaferenceFile file);
        Response<BlobContentInfo> Upload(string path);
        Response<BlobContentInfo> Upload(Stream conteudo);
        Response<bool> Delete(string nomeBlob);
        Response<bool> Delete(BlobClient blobClient);
        AsyncPageable<BlobItem> GetAllAsync();

    }

    public class AzureStorage: IAzureStorage
    {
        private readonly BlobContainerClient _container;
        public AzureStorage(string connectionString, string nomeContainer, ILocalFileHandler fileHandler)
        {
            _container = new BlobContainerClient(connectionString, nomeContainer);
            _fileHandler = fileHandler;

            _container.CreateIfNotExists();
        }

        public ILocalFileHandler _fileHandler { get; }

        public Response<bool> Delete(string nomeBlob)
        {
            var blob = _container.GetBlobClient(nomeBlob);

            return blob.DeleteIfExists();
        }

        public Response<bool> Delete(BlobClient blobClient)
        {
            return blobClient.DeleteIfExists();
        }

        public BlobClient Get(string nome)
        {
            return _container.GetBlobClient(nome);
        }

        public BlobClient Get(TransaferenceFile file)
        {
            return Get(file.Name);
        }

        public Pageable<BlobItem> GetAll()
        {
            return _container.GetBlobs();
        }
        public AsyncPageable<BlobItem> GetAllAsync()
        {
            return  _container.GetBlobsAsync();
        }
        public Response<BlobContentInfo> Upload(TransaferenceFile file)
        {
            _container.DeleteBlobIfExists(file.Name);

            return _container.UploadBlob(file.Name, file.Content);
        }

        public Response<BlobContentInfo> Upload(string path)
        {
            var file = _fileHandler.Read(path, out Stream outputFile) ? outputFile : null;
            var splitedPath = path.Split('\\');

            var fileName = splitedPath[splitedPath.Length - 1];

            _container.DeleteBlobIfExists(fileName);

            return _container.UploadBlob(fileName, file);
        }

        public Response<BlobContentInfo> Upload(Stream conteudo)
        {
            var nomeGerado = Guid.NewGuid().ToString();

            return _container.UploadBlob(nomeGerado, conteudo);
        }
    }
}
