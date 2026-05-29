using Microsoft.EntityFrameworkCore;
using PDFStore.Core.Domain.Entities;
using PDFStore.Core.Interfaces;

namespace PDFStore.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private DocumentContext _context;

        public DocumentRepository(DocumentContext context)
        {
            _context = context;
        }

        public async Task<DocumentItem> Insert(DocumentItem document)
        {
            var entry = await _context.AddAsync(document);

            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<DocumentItem?> GetById(Guid id)
        {
            return await _context.FindAsync<DocumentItem>(id);
        }

        public async Task<DocumentItem?> GetByHash(string sha256)
        {
            return await _context.DocumentItems.FirstOrDefaultAsync(x => x.ContentHash == sha256);
        }

        public async Task<IEnumerable<DocumentItem>> GetAllByFileName(string fileName, int? limit = null)
        {
            var query = _context.DocumentItems.Where(x => x.FileName == fileName);

            if (limit != null)
            {
                query = query.Take((int)limit);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<DocumentItem>> GetAll(int? limit = null)
        {
            var query = _context.DocumentItems.AsQueryable();

            if (limit != null)
            {
                query = query.Take((int)limit);
            }

            return await query.ToListAsync();
        }
    }
}