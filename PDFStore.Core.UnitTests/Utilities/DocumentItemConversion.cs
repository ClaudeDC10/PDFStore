using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Domain.Entities;

namespace PDFStore.Core.UnitTests.Utilities
{
    public static class DocumentItemConversion
    {
        public static Document ConvertToDocumentItem(DocumentItem item)
            => new(item.Id, item.FileName, item.Content);

        public static IEnumerable<Document> ConvertToDocumentItem(IEnumerable<DocumentItem> items)
            => items.Select(ConvertToDocumentItem);
    }
}