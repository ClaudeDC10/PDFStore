using PDFStore.Core.Interfaces;
using UglyToad.PdfPig;

namespace PDFStore.Core.Services
{
    public class PdfReaderService : IPdfReaderService
    {
        public Task<string> PdfToString(Stream stream)
        {
            string content = string.Empty;

            using (PdfDocument document = PdfDocument.Open(stream))
            {
                foreach (var page in document.GetPages())
                {
                    content += page.Text;
                }
            }

            return Task.FromResult(content);
        }
    }
}