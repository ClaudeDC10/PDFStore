using Microsoft.AspNetCore.Mvc;
using PDFStore.Core.Domain.Contracts;
using PDFStore.Core.Interfaces;

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
        public async Task<ActionResult<Document>> PostUploadDocument(IFormFile pdf)
        {
            try
            {
                var result = await _uploadService.Upload(pdf.FileName, pdf.OpenReadStream());
                return Ok(result);
            }
            catch (InvalidOperationException error)
            {
                return Conflict(error.Message);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something went wrong: {error.Message}");
            }
        }

        [HttpGet("retrieve/{id}")]
        public async Task<ActionResult<Document>> GetRetrieveDocumentById(Guid id)
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something went wrong: {error.Message}");
            }
        }

        [HttpGet("retrieve/all/{fileName}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetRetrieveAllDocumentsByFileName(string fileName, int? limit = null)
        {
            try
            {
                var result = await _retrievalService.GetAllByFileName(fileName, limit);
                return Ok(result);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something went wrong: {error.Message}");
            } 
        }

        [HttpGet("retrieve/all")]
        public async Task<ActionResult<IEnumerable<Document>>> GetRetrieveAllDocuments(int? limit = null)
        {
            try
            {
                var result = await _retrievalService.GetAll(limit);
                return Ok(result);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something went wrong: {error.Message}");
            } 
        }
    }
}