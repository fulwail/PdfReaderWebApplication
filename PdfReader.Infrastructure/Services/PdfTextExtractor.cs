using System.Text;
using Microsoft.Extensions.Logging;
using UglyToad.PdfPig;

namespace PdfReader.Infrastructure.Services;

public class PdfTextExtractor(ILogger<PdfTextExtractor> logger): IPdfTextExtractor
{
    public async Task<string> ExtractTextAsync(byte[] pdfContent)
    {
        try
        {
            using var stream = new MemoryStream(pdfContent);
            using var document = PdfDocument.Open(stream);
            
            var text = new StringBuilder();
            
            foreach (var page in document.GetPages())
            {
                var pageText = page.Text;
                if (!string.IsNullOrWhiteSpace(pageText))
                {
                    text.AppendLine(pageText);
                }
            }
            
            var extractedText = text.ToString().Trim();
            
            logger.LogInformation("Extracted {TextLength} characters from PDF with {PageCount} pages", 
                extractedText.Length, document.NumberOfPages);
            
            return extractedText;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error extracting text from PDF");
            throw new InvalidOperationException("Failed to extract text from PDF", ex);
        }
    }
}