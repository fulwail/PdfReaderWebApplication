
namespace PdfReader.Core.Models;

public class PdfContentResponses
{
    public string Content { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
}