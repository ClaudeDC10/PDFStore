using System.Text;
using PDFStore.Core.Interfaces;
using UglyToad.PdfPig;

namespace PDFStore.Infrastructure.Services
{
    public class PdfReaderAdapter : IPdfReaderAdapter
    {
        public Task<string> PdfToString(Stream stream)
        {
            var contentBuilder = new StringBuilder();

            using var document = PdfDocument.Open(stream);
            
            foreach (var page in document.GetPages())
            {
                contentBuilder.Append($"{page.Text.TrimEnd()}\f");
            }

            var content = contentBuilder.ToString().TrimEnd('\f');

            return Task.FromResult(content);
        }
    }
}