using Moq;
using Moq.Language.Flow;
using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Interfaces;

namespace PDFStore.Api.UnitTests.Extension
{
    public static class MockRetrievalServiceSetupExtension
    {
        public static IReturnsResult<IRetrievalService> SetupGetDocumentsByIdReturnValue(
            this Mock<IRetrievalService> mock, Document expected)
        {
            return mock.Setup(r => r.GetDocumentById(It.IsAny<Guid>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IRetrievalService> SetupGetDocumentsByIdThrowsException(
            this Mock<IRetrievalService> mock, Exception exception)
        {
            return mock.Setup(r => r.GetDocumentById(It.IsAny<Guid>()))
                       .ThrowsAsync(exception);
        }

        public static IReturnsResult<IRetrievalService> SetupGetRetrieveByFileNameReturnValue(
            this Mock<IRetrievalService> mock, IEnumerable<Document> expected)
        {
            return mock.Setup(r => r.GetFilterByFileName(It.IsAny<string>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IRetrievalService> SetupGetRetrieveByFileNameThrowException(
            this Mock<IRetrievalService> mock, Exception exception)
        {
            return mock.Setup(r => r.GetFilterByFileName(It.IsAny<string>()))
                       .ThrowsAsync(exception);
        }

        public static IReturnsResult<IRetrievalService> SetupGetRetrieveAllDocumentsReturnValue(
            this Mock<IRetrievalService> mock, IEnumerable<Document> expected)
        {
            return mock.Setup(r => r.GetAll()).ReturnsAsync(expected);
        }

        public static IReturnsResult<IRetrievalService> SetupGetRetrieveAllDocumentsThrowException(
            this Mock<IRetrievalService> mock, Exception exception)
        {
            return mock.Setup(r => r.GetAll()).ThrowsAsync(exception);
        }
    }
}