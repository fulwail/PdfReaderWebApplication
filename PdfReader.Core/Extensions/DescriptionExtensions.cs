using System.ComponentModel;
using System.Reflection;

namespace PdfReader.Core.Extensions;

public static class DescriptionExtensions
{
    public static string GetDescription<T>(this T value)
    {
        FieldInfo fieldName = value?.GetType()?.GetField(value?.ToString());


        if (fieldName.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes 
            && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
}