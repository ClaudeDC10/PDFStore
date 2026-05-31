using Shouldly;
using System.Diagnostics.CodeAnalysis;

using PDFStore.Infrastructure.Repositories;
using PDFStore.Core.Domain.Entities;
using PDFStore.Infrastructure.IntegrationTests.Mock;
using PDFStore.Infrastructure.IntegrationTests.Utilities;


namespace PDFStore.Infrastructure.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class DocumentRepositoryTests : IDisposable
    {
        private readonly DocumentRepository _repository;
        private readonly MockDatabase _database;
        private readonly DataSetup _setup;

        public DocumentRepositoryTests()
        {
            _database = new();
            _repository = new(_database.Context);
            _setup = new(_database.Context);
        }

        [Fact]
        public async Task Insert_WithDocument_ReturnDocumentItem()
        {
            // Arrange
            var document = new DocumentItem(Guid.Empty, "test.pdf", "someHash", "test");

            // Act
            var result = await _repository.Insert(document);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(document);
        }

        [Fact]
        public async Task GetById_WithExisting_ReturnDocumentItem()
        {
            // Arrange
            await _setup.AddDefaultData();
            var ids = await _setup.GetDefaultIds();

            // Act
            var result = await _repository.GetById(ids.First());

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetById_NotExists_ReturnNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _repository.GetById(id);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GetByHash_WithExisting_ReturnDocumentItem()
        {
            // Arrange
            await _setup.AddDefaultData();
            var hash = await _setup.GetDefaultContentHashes();

            // Act
            var result = await _repository.GetByHash(hash.First());

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetByHash_NotExists_ReturnNull()
        {
            // Arrange
            var hash = Guid.NewGuid().ToString();

            // Act
            var result = await _repository.GetByHash(hash);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GetFilterByFileName_WithExistingAndNoLimit_ReturnDocumentItems()
        {
            // Arrange
            await _setup.AddDefaultData();
            var fileNames = await _setup.GetDefaultFileNames();
            var filter = fileNames.First().First().ToString();
            var expectedCount = fileNames.Count(f => f.Contains(filter));

            // Act
            var result = await _repository.GetFilterByFileName(filter);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(expectedCount);
        }

        [Fact]
        public async Task GetFilterByFileName_NoHitNoLimit_ReturnEmpty()
        {
            // Arrange
            await _setup.AddDefaultData();
            var filter = Guid.NewGuid().ToString();

            // Act
            var result = await _repository.GetFilterByFileName(filter);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetFilterByFileName_AllWithLimit_ReturnCorrectCount()
        {
            // Arrange
            await _setup.AddDefaultData();
            var allCount = _setup.DefaultItemCount;
            var limit = allCount - 2;

            // Act
            var result = await _repository.GetFilterByFileName(string.Empty, limit);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(limit);
        }

        [Fact]
        public async Task GetAll_WithoutLimit_ReturnDocumentItems()
        {
            // Arrange
            await _setup.AddDefaultData();
            var expectedCount = _setup.DefaultItemCount;

            // Act
            var result = await _repository.GetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(expectedCount);
        }

        [Fact]
        public async Task GetAll_WithLimit_ReturnCorrectCount()
        {
            // Arrange
            await _setup.AddDefaultData();
            var allCount = _setup.DefaultItemCount;
            var limit = allCount - 2;

            // Act
            var result = await _repository.GetAll(limit);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(limit);
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
