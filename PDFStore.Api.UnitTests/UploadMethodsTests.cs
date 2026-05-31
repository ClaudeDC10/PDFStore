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
    public class UploadMethodsTests
    {
        private readonly Mock<IUploadService> _uploadService;
        private readonly Mock<IRetrievalService> _retrievalService;
        private readonly IFormFile _mockFile;
        private const string _mockError = "Mock error";

        public UploadMethodsTests()
        {
            _retrievalService = new();
            _uploadService = new();
            _mockFile = new Mock<IFormFile>().Object;
        }


        [Fact]
        public async Task PostUploadDocument_WithValidFile_ReturnOk()
        {
            // Arrange
            var mockDocument = new Document(Guid.Empty, "test.pdf", "test");
            _uploadService.SetupUploadReturnValue(mockDocument);

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.PostUploadDocument(_mockFile);
            

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            
            var value = ((OkObjectResult)result.Result).Value;
            value.ShouldBeOfType<Document>();

            var document = (Document)value;
            document.ShouldBeEquivalentTo(mockDocument);
        }

        [Fact]
        public async Task PostUploadDocument_WithDuplicate_ReturnConflict()
        {
            // Arrange
            _uploadService.SetupUploadThrowsException(
                new InvalidOperationException(_mockError));

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.PostUploadDocument(_mockFile);

            // Assert
            result.Result.ShouldBeOfType<ConflictObjectResult>();

            var objectResult = (ConflictObjectResult)result.Result;
            objectResult.Value.ShouldBeEquivalentTo(_mockError);
        }

        [Fact]
        public async Task PostUploadDocument_WithInvalidType_ReturnUnsupported()
        {
            // Arrange
            _uploadService.SetupUploadThrowsException(
                new InvalidDataException(_mockError));

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.PostUploadDocument(_mockFile);

            // Assert
            result.Result.ShouldBeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result.Result;
            objectResult.StatusCode.ShouldBe(StatusCodes.Status415UnsupportedMediaType);
            objectResult.Value.ShouldBe($"Only accepts PDF files: {_mockError}");
        }

        [Fact]
        public async Task PostUploadDocument_WithServerIssue_ReturnInternalServerError()
        {
            // Arrange            
            _uploadService.SetupUploadThrowsException(
                new Exception(_mockError));

            var sut = new DocumentsController(_uploadService.Object, _retrievalService.Object);

            // Act
            var result = await sut.PostUploadDocument(_mockFile);

            // Assert
            result.Result.ShouldBeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result.Result;
            objectResult.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
            objectResult.Value.ShouldBe($"Something went wrong: {_mockError}");
        }
    }
}