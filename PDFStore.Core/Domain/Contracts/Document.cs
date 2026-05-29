namespace PDFStore.Core.Domain.Contracts
{
    public class Document
    {
        public Guid Id {get; set; }

        public string FileName { get; set; }

        public string Content { get; set; }

        public Document(Guid id, string fileName, string content)
        {
            Id = id;
            FileName = fileName;
            Content = content;
        }
    }
}