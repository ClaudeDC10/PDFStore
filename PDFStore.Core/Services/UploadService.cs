using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Domain.Entities;
using PDFStore.Core.Interfaces;
using System.Security.Cryptography;

namespace PDFStore.Core.Services
{
    public class UploadService : IUploadService
    {
        private IDocumentRepository _repository;
        private IPdfReaderService _readerService;

        public UploadService(IDocumentRepository repository, IPdfReaderService readerService)
        {
            _repository = repository;
            _readerService = readerService;
        }

        public async Task<Document> Upload(string fileName, Stream stream)
        {
            var hash = await SHA256.HashDataAsync(stream);
            var hexString = Convert.ToHexString(hash);

            if (await _repository.GetByHash(hexString) != null)
            {
                throw new InvalidOperationException("Duplicate data found");
            }

            stream.Position = 0;

            var content = await _readerService.PdfToString(stream);

            var item = await _repository.Insert(
                new DocumentItem(Guid.Empty, fileName, hexString, content));

            return new Document(item.Id, item.FileName, item.Content);
        }
    }
}