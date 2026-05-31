using Moq;
using Shouldly;

using PDFStore.Core.Interfaces;
using PDFStore.Core.Domain.Entities;
using PDFStore.Core.UnitTests.Extensions;
using PDFStore.Core.Services;
using PDFStore.Core.Domain.Contracts;
using UglyToad.PdfPig.Core;
using System.Diagnostics.CodeAnalysis;

namespace PDFStore.Core.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class UploadServiceTests
    {
        private readonly Mock<IDocumentRepository> _repository;
        private readonly Mock<IPdfReaderAdapter> _readerService;
        private readonly MemoryStream _stream;

        public UploadServiceTests()
        {
            _repository = new();
            _readerService = new();
            _stream = new();
        }

        [Theory]
        [InlineData("test.pdf", "test")]
        [InlineData("test.pdf", "")]
        [InlineData(".pdf", "test")]
        public async Task Upload_WithValidFile_ReturnsDocument(string fileName, string content)
        {
            // Arrange
            var mockItem = new DocumentItem(Guid.Empty, fileName, "someHash", content);
            var expected = new Document(mockItem.Id, fileName, content);

            _repository.SetupGetByHashReturnNull();
            _repository.SetupInsertReturnExpected(mockItem);
            _readerService.SetupPdfToStringReturnValue(content);

            var sut = new UploadService(_repository.Object, _readerService.Object);

            // Act
            var result = await sut.Upload(fileName, _stream);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public async Task Upload_WithDuplicate_ThrowsInvalidOperationException()
        {
            // Arrange
            var expected = "Duplicate data found.";

            _repository.SetupGetByHashReturnValue();

            var sut = new UploadService(_repository.Object, _readerService.Object);
            
            // Act
            var error = await Should.ThrowAsync<InvalidOperationException>(() =>
                sut.Upload("test.pdf", _stream));

            // Assert
            error.Message.ShouldBe(expected);
        }

        [Fact]
        public async Task Upload_WithNotPdf_ThrowsInvalidDataException()
        {
            // Arrange
            var pdfException = new PdfDocumentFormatException("Mocked Format Exception");
            var expectedMessage = $"Invalid file type uploaded: {pdfException.Message}";

            _repository.SetupGetByHashReturnNull();
            _readerService.SetupPdfToStringThrowsException(pdfException);

            var sut = new UploadService(_repository.Object, _readerService.Object);

            // Act
            var error = await Should.ThrowAsync<InvalidDataException>(() =>
                sut.Upload("test.txt", _stream));

            // Assert
            error.Message.ShouldBe(expectedMessage);
        }
    }
}