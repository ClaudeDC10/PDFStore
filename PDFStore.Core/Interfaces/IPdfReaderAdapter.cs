namespace PDFStore.Core.Interfaces
{
    public interface IPdfReaderAdapter
    {
        public Task<string> PdfToString(Stream stream);
    }
}