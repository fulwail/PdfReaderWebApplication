using System.ComponentModel;

namespace PdfReader.Core.Enums;

public enum ProcessingStatus
{
    [Description("Загружен")]
    Uploaded,
    [Description("Обрабатывается")]
    Processing,
    [Description("Завершен")]
    Completed,
    [Description("Ошибка")]
    Failed
}