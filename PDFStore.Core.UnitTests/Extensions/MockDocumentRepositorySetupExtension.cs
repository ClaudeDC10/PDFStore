using Moq;
using Moq.Language.Flow;
using PDFStore.Core.Domain.Entities;
using PDFStore.Core.Interfaces;

namespace PDFStore.Core.UnitTests.Extensions
{
    public static class MockDocumentRepositorySetupExtension
    {
        public static IReturnsResult<IDocumentRepository> SetupGetByHashReturnNull(
            this Mock<IDocumentRepository> mock)
        {
            return mock.Setup(r => r.GetByHash(It.IsAny<string>()))
                       .ReturnsAsync((DocumentItem?)null);
        }

        public static IReturnsResult<IDocumentRepository> SetupGetByHashReturnValue(
            this Mock<IDocumentRepository> mock)
        {
            return mock.Setup(r => r.GetByHash(It.IsAny<string>()))
                       .ReturnsAsync(new DocumentItem(Guid.Empty, string.Empty, string.Empty, string.Empty));
        }

        public static IReturnsResult<IDocumentRepository> SetupInsertReturnExpected(
            this Mock<IDocumentRepository> mock, DocumentItem expected)
        {
            return mock.Setup(r => r.Insert(It.IsAny<DocumentItem>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IDocumentRepository> SetupGetByIdReturnValue(
            this Mock<IDocumentRepository> mock, DocumentItem expected)
        {
            return mock.Setup(r => r.GetById(It.IsAny<Guid>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IDocumentRepository> SetupGetByIdReturnNull(
            this Mock<IDocumentRepository> mock)
        {
            return mock.Setup(r => r.GetById(It.IsAny<Guid>()))
                       .ReturnsAsync((DocumentItem?)null);
        }

        public static IReturnsResult<IDocumentRepository> SetupGetAllByFileNameReturnValues(
            this Mock<IDocumentRepository> mock, IEnumerable<DocumentItem> expected)
        {
            return mock.Setup(r => r.GetFilterByFileName(It.IsAny<string>()))
                       .ReturnsAsync(expected);
        }

        public static IReturnsResult<IDocumentRepository> SetupGetAllByFileNameReturnEmpty(
            this Mock<IDocumentRepository> mock)
        {
            return mock.Setup(r => r.GetFilterByFileName(It.IsAny<string>()))
                       .ReturnsAsync(Enumerable.Empty<DocumentItem>());
        }

        public static IReturnsResult<IDocumentRepository> SetupGetAllReturnValues(
            this Mock<IDocumentRepository> mock, IEnumerable<DocumentItem> expected)
        {
            return mock.Setup(r => r.GetAll()).ReturnsAsync(expected);
        }

        public static IReturnsResult<IDocumentRepository> SetupGetAllReturnEmpty(
            this Mock<IDocumentRepository> mock)
        {
            return mock.Setup(r => r.GetAll()).ReturnsAsync(Enumerable.Empty<DocumentItem>());
        }
    }
}