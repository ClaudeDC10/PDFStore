using Microsoft.EntityFrameworkCore;
using PDFStore.Core.Interfaces;
using PDFStore.Data;
using PDFStore.Data.Repositories;

namespace PDFStore.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPdfStoreData(this IServiceCollection serviceDescriptors, IConfiguration config)
        {
            serviceDescriptors.AddDbContext<DocumentContext>(options =>
                options.UseSqlite(config.GetConnectionString("PdfStoreDb")));
            serviceDescriptors.AddScoped<IDocumentRepository, DocumentRepository>();

            return serviceDescriptors;
        }
    }
}