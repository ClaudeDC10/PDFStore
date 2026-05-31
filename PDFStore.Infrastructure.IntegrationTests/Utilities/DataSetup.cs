using PDFStore.Core.Domain.Entities;

namespace PDFStore.Infrastructure.IntegrationTests.Utilities
{
    public class DataSetup
    {
        private DocumentContext _context;

        private readonly List<DocumentItem> _defaultItems;

        public int DefaultItemCount { get; }

        public DataSetup(DocumentContext context, int defaultItemCount = 5)
        {
            _context = context;
            _defaultItems = [];
            DefaultItemCount = defaultItemCount;
            
            for (int i = 0; i < defaultItemCount; i++)
            {
                _defaultItems.Add(new DocumentItem(Guid.Empty, randomizedString(), randomizedString(), randomizedString()));
            }
        }

        public async Task AddDefaultData()
        {
            await _context.DocumentItems.AddRangeAsync(_defaultItems);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Guid>> GetDefaultIds() => 
            _defaultItems.Select(i => i.Id);

        public async Task<IEnumerable<string>> GetDefaultFileNames() =>
            _defaultItems.Select(i => i.FileName);

        public async Task<IEnumerable<string>> GetDefaultContentHashes() =>
            _defaultItems.Select(i => i.ContentHash);

        private string randomizedString() => Guid.NewGuid().ToString();
    }
}