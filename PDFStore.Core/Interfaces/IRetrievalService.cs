using PDFStore.Core.Domain.Contracts;

namespace PDFStore.Core.Interfaces
{
    public interface IRetrievalService
    {
        public Task<Document> GetDocumentById(Guid id);
        public Task<IEnumerable<Document>> GetFilterByFileName(string filename, int? limit = null);
        public Task<IEnumerable<Document>> GetAll(int? limit = null);
    }
}