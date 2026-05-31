using Moq;
using Moq.Language.Flow;
using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Interfaces;

namespace PDFStore.Api.UnitTests.Extension
{
    public static class MockUploadServiceSetupExtension
    {
        public static IReturnsResult<IUploadService> SetupUploadReturnValue(
            this Mock<IUploadService> mock, Document expected)
        {
            return mock.Setup(u => u.Upload(It.IsAny<string>(), It.IsAny<Stream>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IUploadService> SetupUploadThrowsException(
            this Mock<IUploadService> mock, Exception exception)
        {
            return mock.Setup(u => u.Upload(It.IsAny<string>(), It.IsAny<Stream>()))
                       .ThrowsAsync(exception);
        }
    }
}