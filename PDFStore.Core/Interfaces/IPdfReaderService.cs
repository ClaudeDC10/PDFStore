namespace PDFStore.Core.Interfaces
{
    public interface IPdfReaderService
    {
        public Task<string> PdfToString(Stream stream);
    }
}