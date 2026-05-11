using MassTransit;
using PdfReader.Core.Enums;
using PdfReader.Core.Models;
using PdfReader.Infrastructure.Services;

namespace PdfReader.Worker.Consumers;

public class PdfUploadedConsumer(IPdfDocumentService service, 
    IPdfTextExtractor pdfTextExtractor,
    ILogger<PdfUploadedConsumer> logger): IConsumer<PdfUploadedEvent>
{
    public async Task Consume(ConsumeContext<PdfUploadedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing PDF document: {DocumentId} - {FileName}", message.DocumentId, message.FileName);

        try
        {
            await service.ChangeStatus(message.DocumentId, ProcessingStatus.Processing);
            
            var extractedText = await pdfTextExtractor.ExtractTextAsync(message.FileContent);
            await service.UpdateExtractedText(message.DocumentId, extractedText);
            await service.ChangeStatus(message.DocumentId, ProcessingStatus.Completed);
            
            logger.LogInformation("Successfully processed PDF: {DocumentId}, extracted {TextLength} characters", 
                message.DocumentId, extractedText.Length);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process PDF: {DocumentId}", message.DocumentId);
            await service.ChangeStatus(message.DocumentId, ProcessingStatus.Failed, ex.Message);
            
            throw; 
        }
      
    }
}