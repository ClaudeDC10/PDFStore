using PDFStore.Core.Domain.Entities;

namespace PDFStore.Core.Interfaces
{
    public interface IDocumentRepository
    {
        public Task<DocumentItem> Insert(DocumentItem document);

        public Task<DocumentItem?> GetById(Guid id);

        public Task<DocumentItem?> GetByHash(string sha256);

        public Task<IEnumerable<DocumentItem>> GetAllByFileName(string filename, int? limit = null);

        public Task<IEnumerable<DocumentItem>> GetAll(int? limit = null);
    }
}