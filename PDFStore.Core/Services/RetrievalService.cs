using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Domain.Entities;
using PDFStore.Core.Interfaces;

namespace PDFStore.Core.Services
{
    public class RetrievalService : IRetrievalService
    {
        private IDocumentRepository _repository;

        public RetrievalService(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Document> GetDocumentById(Guid id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"No item found with id {id}");
            }

            return convertToDocument(item);
        }

        public async Task<IEnumerable<Document>> GetAllByFileName(string fileName, int? limit = null)
        {
            var items = await _repository.GetAllByFileName(fileName, limit);
            return convertToDocument(items);
        }

        public async Task<IEnumerable<Document>> GetAll(int? limit = null)
        {
            var items = await _repository.GetAll(limit);
            return convertToDocument(items);
        }

        private Document convertToDocument(DocumentItem item)
        {
            return new Document(item.Id, item.FileName, item.Content);
        }

        private IEnumerable<Document> convertToDocument(IEnumerable<DocumentItem> items)
        {
            return items.Select(convertToDocument);
        }
    }
}