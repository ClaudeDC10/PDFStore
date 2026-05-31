using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Domain.Entities;
using PDFStore.Core.Interfaces;
using System.Security.Cryptography;
using UglyToad.PdfPig.Core;

namespace PDFStore.Core.Services
{
    public class UploadService : IUploadService
    {
        private readonly IDocumentRepository _repository;
        private readonly IPdfReaderAdapter _readerService;

        public UploadService(IDocumentRepository repository, IPdfReaderAdapter readerService)
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
                throw new InvalidOperationException("Duplicate data found.");
            }

            stream.Position = 0;
            string content;

            try
            {
                content = await _readerService.PdfToString(stream);
            }
            catch (PdfDocumentFormatException error)
            {
                throw new InvalidDataException($"Invalid file type uploaded: {error.Message}");
            }
            

            var item = await _repository.Insert(
                new DocumentItem(Guid.Empty, fileName, hexString, content));

            return new Document(item.Id, item.FileName, item.Content);
        }
    }
}