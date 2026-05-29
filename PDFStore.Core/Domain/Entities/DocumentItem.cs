namespace PDFStore.Core.Domain.Entities
{
    public class DocumentItem
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string ContentHash { get; set; }

        public string Content { get; set; }

        public DocumentItem(Guid id, string fileName, string contentHash, string content)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            FileName = fileName;
            ContentHash = contentHash;
            Content = content;
        }
    }
}