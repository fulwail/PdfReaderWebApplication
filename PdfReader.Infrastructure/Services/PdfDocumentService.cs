using Mapster;
using Microsoft.EntityFrameworkCore;
using PdfReader.Core.Enums;
using PdfReader.Core.Models;
using PdfReader.Infrastructure.Context;

namespace PdfReader.Infrastructure.Services;

public class PdfDocumentService(PdfReaderDbContext context): IPdfDocumentService
{
    public async Task<PdfDocumentInfo> Create(PdfDocumentEntity entity)
    {
        await context.Documents.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity.Adapt<PdfDocumentInfo>();
    }

    public async Task ChangeStatus(Guid documentId, ProcessingStatus status, string? errorMessage = null)
    {
        var document = await context.Documents.FindAsync(documentId);
        if (document != null)
        {
            document.Status = status;
            document.ErrorMessage = errorMessage;
            await context.SaveChangesAsync();
        }
    }

    public async Task<PdfContentResponses> GetPdfContent(Guid id)
    {
        var document = await context.Documents.FirstOrDefaultAsync(x => x.Id == id);
        if (document == null|| string.IsNullOrEmpty(document.ExtractedText))
        {
            return new PdfContentResponses()
            {
                ErrorMessage = "File"
            };
        };
        return new PdfContentResponses()
        {
            Content = document.ExtractedText
        };
    }

    public async Task UpdateExtractedText(Guid id, string text)
    {
        var document = await context.Documents.FindAsync(id);
        if (document != null)
        {
            document.ExtractedText = text;
            document.ProcessedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<PdfDocumentInfo>> GetPdfDocumentList()
    {
        var entities= await context.Documents.ToListAsync();
        return entities.Adapt<List<PdfDocumentInfo>>();
    }
}