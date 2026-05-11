using PdfReader.Core.Enums;

namespace PdfReader.Core.Models;

public class PdfDocumentInfo
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public ProcessingStatus Status { get; set; }  
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string ErrorMessage { get; set; }
}