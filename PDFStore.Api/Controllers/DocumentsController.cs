using Microsoft.AspNetCore.Mvc;
using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Interfaces;
using PDFStore.Api.Domain.Models;

namespace PDFStore.Api.Controllers
{   
    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        private readonly IRetrievalService _retrievalService;

        public DocumentsController(IUploadService uploadService, 
                                   IRetrievalService retrievalService)
        {
            _uploadService = uploadService;
            _retrievalService = retrievalService;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Document>> PostUploadDocument([FromForm] FileUploadModel file)
        {
            try
            {
                var pdf = file.FormFile;
                var result = await _uploadService.Upload(pdf.FileName, pdf.OpenReadStream());
                return Ok(result);
            }
            catch (InvalidOperationException error)
            {
                return Conflict(error.Message);
            }
            catch (InvalidDataException error)
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, 
                    $"Only accepts PDF files: {error.Message}");
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Something went wrong: {error.Message}");
            }
        }

        [HttpGet("retrieve/{id}")]
        public async Task<ActionResult<Document>> GetRetrieveById([FromRoute] Guid id)
        {
            try
            {
                var result = await _retrievalService.GetDocumentById(id);
                return Ok(result);
            }
            catch (KeyNotFoundException error)
            {
                return NotFound(error.Message);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Something went wrong: {error.Message}");
            }
        }

        [HttpGet("retrieve/all/{fileName}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetRetrieveFilterByFileName([FromRoute] string fileName, [FromQuery] int? limit = null)
        {
            try
            {
                var result = await _retrievalService.GetFilterByFileName(fileName, limit);
                return Ok(result);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Something went wrong: {error.Message}");
            } 
        }

        [HttpGet("retrieve/all")]
        public async Task<ActionResult<IEnumerable<Document>>> GetRetrieveAllDocuments([FromQuery] int? limit = null)
        {
            try
            {
                var result = await _retrievalService.GetAll(limit);
                return Ok(result);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Something went wrong: {error.Message}");
            } 
        }
    }
}