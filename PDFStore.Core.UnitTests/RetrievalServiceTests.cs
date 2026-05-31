using Moq;
using Shouldly;

using PDFStore.Core.Domain.Entities;
using PDFStore.Core.Interfaces;
using PDFStore.Core.Services;
using PDFStore.Core.UnitTests.Extensions;
using PDFStore.Core.UnitTests.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace PDFStore.Core.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class RetrievalServiceTests
    {
        private readonly Mock<IDocumentRepository> _repository;

        public RetrievalServiceTests()
        {
            _repository = new();
        }

        [Theory]
        [InlineData("test.pdf", "test")]
        [InlineData("test.pdf", "")]
        [InlineData(".pdf", "test")]
        public async Task GetDocumentById_WithExisting_ReturnsDocument(string fileName, string content)
        {
            // Arrange
            var mockItem = new DocumentItem(Guid.Empty, fileName, "someHash", content);
            var expected = DocumentItemConversion.ConvertToDocumentItem(mockItem);

            _repository.SetupGetByIdReturnValue(mockItem);

            var sut = new RetrievalService(_repository.Object);

            // Act
            var result = await sut.GetDocumentById(expected.Id);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetDocumentById_NotExisting_ThrowsKeyNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedMessage = $"No item found with id {id}.";

            _repository.SetupGetByIdReturnNull();

            var sut = new RetrievalService(_repository.Object);

            // Act
            var error = await Should.ThrowAsync<KeyNotFoundException>(() =>
                sut.GetDocumentById(id));

            // Assert
            error.Message.ShouldBe(expectedMessage);
        }

        [Theory]
        [InlineData("test.pdf", 1)]
        [InlineData("test_multiple.pdf", 3)]
        public async Task GetAllByFileName_WithData_ReturnDocuments(string fileName, int itemCount)
        {
            // Arrange
            List<DocumentItem> mockItems = [];
            for (int i = 0; i < itemCount; i++)
            {
                mockItems.Add(new DocumentItem(Guid.Empty, fileName, "someHash", "test"));
            }
            var expectedItems = DocumentItemConversion.ConvertToDocumentItem(mockItems);

            _repository.SetupGetAllByFileNameReturnValues(mockItems);

            var sut = new RetrievalService(_repository.Object);

            // Act
            var result = await sut.GetFilterByFileName(fileName);

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldBeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task GetAllByFileName_WithNoData_ReturnEmpty()
        {
            // Arrange
            _repository.SetupGetAllByFileNameReturnEmpty();

            var sut = new RetrievalService(_repository.Object);

            // Act
            var result = await sut.GetFilterByFileName("test.pdf");

            // Assert
            result.ShouldBeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async Task GetAll_WithData_ReturnDocuments(int itemCount)
        {
            // Arrange
            List<DocumentItem> mockItems = [];
            for (int i = 0; i < itemCount; i++)
            {
                mockItems.Add(new DocumentItem(Guid.Empty, "test.pdf", "someHash", "test"));
            }
            var expectedItems = DocumentItemConversion.ConvertToDocumentItem(mockItems);

            _repository.SetupGetAllReturnValues(mockItems);

            var sut = new RetrievalService(_repository.Object);

            // Act
            var result = await sut.GetAll();

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldBeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task GetAll_WithNoData_ReturnEmpty()
        {
            // Arrange
            _repository.SetupGetAllReturnEmpty();

            var sut = new RetrievalService(_repository.Object);

            // Act
            var result = await sut.GetAll();

            // Assert
            result.ShouldBeEmpty();
        }
    }
}