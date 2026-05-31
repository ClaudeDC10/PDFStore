using Moq;
using Moq.Language.Flow;
using PDFStore.Core.Interfaces;

namespace PDFStore.Core.UnitTests.Extensions
{
    public static class MockPdfReaderServiceSetupExtension
    {
        public static IReturnsResult<IPdfReaderAdapter> SetupPdfToStringReturnValue(
            this Mock<IPdfReaderAdapter> mock, string expected)
        {
            return mock.Setup(r => r.PdfToString(It.IsAny<Stream>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IPdfReaderAdapter> SetupPdfToStringThrowsException(
            this Mock<IPdfReaderAdapter> mock, Exception exception)
        {
            return mock.Setup(r => r.PdfToString(It.IsAny<Stream>()))
                       .ThrowsAsync(exception);
        }
    }
}