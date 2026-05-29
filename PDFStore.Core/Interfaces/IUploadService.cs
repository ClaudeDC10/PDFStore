using PDFStore.Core.Domain.Contracts;

namespace PDFStore.Core.Interfaces
{
    public interface IUploadService
    {
        public Task<Document> Upload(string filename, Stream stream);
    }
}