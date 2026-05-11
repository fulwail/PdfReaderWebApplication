namespace PdfReader.Core.Models;

public record PdfUploadedEvent
{
    public Guid DocumentId { get; init; }
    public string FileName { get; init; }
    public byte[] FileContent { get; init; }
    public DateTime UploadedAt { get; init; }
}