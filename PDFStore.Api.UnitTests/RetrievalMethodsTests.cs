using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PDFStore.Api.Controllers;
using PDFStore.Api.UnitTests.Extension;
using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Interfaces;
using Shouldly;

namespace PDFStore.Api.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class RetrievalMethodsTests
    {
        private readonly Mock<IUploadService> _uploadService;
        private readonly Mock<IRetrievalService> _retrievalService;
        private readonly Guid _mockId;
        private const string _mockError = "Mock error";
        private const string _mockFileName = "Mock.pdf";

        public RetrievalMethodsTests()
        {
            _uploadService = new();
            _retrievalService = new();

            _mockId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetRetrieveDocumentById_WithExisting_ReturnOk()
        {
            // Arrange
            var expectedDocument = new Document(_mockId, "test.pdf", "test");
            _retrievalService.SetupGetDocumentsByIdReturnValue(expectedDocument);

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveById(_mockId);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(expectedDocument);
        }

        [Fact]
        public async Task GetRetrieveDocumentById_WithNotExists_ReturnNotFound()
        {
            // Arrange
            _retrievalService.SetupGetDocumentsByIdThrowsException(
                new KeyNotFoundException(_mockError));
            
            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveById(_mockId);

            // Assert
            result.Result.ShouldBeOfType<NotFoundObjectResult>();
            var objectResult = (NotFoundObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(_mockError);
        }

        [Fact]
        public async Task GetRetrieveDocumentById_WithServerIssue_ReturnInternalServerError()
        {
            // Arrange
            _retrievalService.SetupGetDocumentsByIdThrowsException(
                new Exception(_mockError));

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveById(_mockId);

            // Assert
            result.Result.ShouldBeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result.Result;
            objectResult.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
            objectResult.Value.ShouldBeEquivalentTo($"Something went wrong: {_mockError}");
        }

        [Fact]
        public async Task GetRetrieveByFileName_WithExisting_ReturnOk()
        {
            // Arrange
            var documents = new List<Document>();

            for(int i = 0; i < 3; i++)
            {
                documents.Add(new Document(Guid.Empty, _mockFileName, "test"));
            }

            _retrievalService.SetupGetRetrieveByFileNameReturnValue(documents);

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveFilterByFileName(_mockFileName);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(documents);
        }

        [Fact]
        public async Task GetRetrieveByFileName_WithNoHit_ReturnOk()
        {
            // Arrange
            _retrievalService.SetupGetRetrieveByFileNameReturnValue([]);

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveFilterByFileName(_mockFileName);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(Enumerable.Empty<Document>());
        }

        [Fact]
        public async Task GetRetrieveByFileName_WithServerIssue_ReturnInternalServerError()
        {
            // Arrange
            _retrievalService.SetupGetRetrieveByFileNameThrowException(
                new Exception(_mockError));
            
            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveFilterByFileName(_mockFileName);

            // Assert
            result.Result.ShouldBeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result.Result;
            objectResult.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
            objectResult.Value.ShouldBeEquivalentTo($"Something went wrong: {_mockError}");
        }

        [Fact]
        public async Task GetRetrieveAllDocuments_WithExisting_ReturnOk()
        {
            // Arrange
            var documents = new List<Document>();

            for(int i = 0; i < 3; i++)
            {
                documents.Add(new Document(Guid.Empty, _mockFileName, "test"));
            }

            _retrievalService.SetupGetRetrieveAllDocumentsReturnValue(documents);

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveAllDocuments();

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(documents);
        }

        [Fact]
        public async Task GetRetrieveAllDocuments_WithNoHit_ReturnOk()
        {
            // Arrange
            _retrievalService.SetupGetRetrieveAllDocumentsReturnValue([]);

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveAllDocuments();

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(Enumerable.Empty<Document>());
        }

        [Fact]
        public async Task GetRetrieveAllDocuments_WithServerIssue_ReturnInternalServerError()
        {
            // Arrange
            _retrievalService.SetupGetRetrieveAllDocumentsThrowException(
                new Exception(_mockError));
            
            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.GetRetrieveAllDocuments();

            // Assert
            result.Result.ShouldBeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result.Result;
            objectResult.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
            objectResult.Value.ShouldBeEquivalentTo($"Something went wrong: {_mockError}");
        }
    }
}