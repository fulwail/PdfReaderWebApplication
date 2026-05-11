using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PdfReader.Core.Enums;
using PdfReader.Core.Models;
using PdfReader.Infrastructure.Services;

namespace PdfReader.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class PdfReaderController(IPdfDocumentService service,IPublishEndpoint publishEndpoint): ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PdfDocumentInfo>> Upload(IFormFile  pdfDocument,CancellationToken stoppingToken)
    {
        if(pdfDocument.ContentType!="application/pdf")
            return BadRequest("Полученный файл не является pdf документом");
        
        using var ms = new MemoryStream();
        await pdfDocument.CopyToAsync(ms);
        
        var document = new PdfDocumentEntity()
        {
            Id=Guid.NewGuid(),
            FileName=pdfDocument.FileName,
            Content=ms.ToArray(),
            Status = ProcessingStatus.Uploaded,
            CreatedAt=DateTime.Now,
        };
        
       var pdfDocumentInfo= await service.Create(document);
        await publishEndpoint.Publish(new PdfUploadedEvent()
            {
                DocumentId=document.Id,
                FileContent = document.Content,
                UploadedAt = document.CreatedAt
                
            },
            stoppingToken);
        return Ok(pdfDocumentInfo);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PdfDocumentInfo>>> GetList()
    {
        var documents = await service.GetPdfDocumentList();
        return Ok(documents);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<string>> GetContentById(Guid id)
    {
        var result = await service.GetPdfContent(id);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);
        
        return Ok(result.Content);
    }
}