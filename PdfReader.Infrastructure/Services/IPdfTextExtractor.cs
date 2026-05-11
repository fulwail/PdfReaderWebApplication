namespace PdfReader.Infrastructure.Services;

public interface IPdfTextExtractor
{
    Task<string> ExtractTextAsync(byte[] pdfContent);
}