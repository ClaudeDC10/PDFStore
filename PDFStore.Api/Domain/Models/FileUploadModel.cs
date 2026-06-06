namespace PDFStore.Api.Domain.Models
{
    public class FileUploadModel
    {
        public required IFormFile FormFile { get; set; }
    }
}