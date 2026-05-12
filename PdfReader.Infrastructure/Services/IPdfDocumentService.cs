using Microsoft.AspNetCore.Http;
using PdfReader.Core.Enums;
using PdfReader.Core.Models;

namespace PdfReader.Infrastructure.Services;

public interface IPdfDocumentService
{
    public Task<PdfDocumentInfo>  Create(PdfDocumentEntity entity);
    public Task ChangeStatus(Guid documentId, ProcessingStatus status, string? errorMessage = null);
    public Task<PdfContentResponses> GetPdfContent(Guid id);
    public Task UpdateExtractedText(Guid id, string text);
    public Task<IEnumerable<PdfDocumentInfo>> GetPdfDocumentList();
    public Task<PdfDocumentInfo> Upload(IFormFile pdfDocument, CancellationToken stoppingToken);
}