using Shouldly;
using System.Diagnostics.CodeAnalysis;

using PDFStore.Infrastructure.Services;

namespace PDFStore.Infrastructure.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class PdfReaderAdapterTests
    {
        private readonly string _assets;
        public PdfReaderAdapterTests()
        {
            _assets = Path.Join(AppContext.BaseDirectory, "TestAssets");
        }

        [Fact]
        public async Task PdfToString_WithOnePage_ReturnWithValue()
        {
            // Arrange
            var assetFile = Path.Join(_assets, "test_single_page.pdf");
            using var stream = new FileStream(assetFile, FileMode.Open, FileAccess.Read);

            var sut = new PdfReaderAdapter();

            // Act
            var result = await sut.PdfToString(stream);

            // Assert
            result.ShouldContain("This is a test.");
        }

        [Fact]
        public async Task PdfToString_WithMultiplePages_ReturnWithValue()
        {
            // Arrage
            var assetFile = Path.Join(_assets, "test_multi_page.pdf");
            using var stream = new FileStream(assetFile, FileMode.Open, FileAccess.Read);

            var sut = new PdfReaderAdapter();

            // Act
            var result = await sut.PdfToString(stream);

            // Assert
            result.ShouldContain("This is a test page 1.\fThis is a test page 2.");
        }

        [Fact]
        public async Task PdfToString_WithBlankPage_ReturnWithValue()
        {
            // Arrage
            var assetFile = Path.Join(_assets, "test_blank_page.pdf");
            using var stream = new FileStream(assetFile, FileMode.Open, FileAccess.Read);

            var sut = new PdfReaderAdapter();

            // Act
            var result = await sut.PdfToString(stream);

            // Assert
            result.ShouldBeNullOrWhiteSpace();
        }
    }
}