using Mapster;
using PdfReader.Core.Models;

namespace PdfReader.Core.Profiles;

public class PdfDocumentProfiler: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PdfDocumentEntity, PdfDocumentInfo>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FileName, src => src.FileName)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.ProcessedAt, src => src.ProcessedAt)
            .Map(dest => dest.ErrorMessage, src => src.ErrorMessage)
            .IgnoreNonMapped(true)
            .IgnoreNullValues(true);
    }
}