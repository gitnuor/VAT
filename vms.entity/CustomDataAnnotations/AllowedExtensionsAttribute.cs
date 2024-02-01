using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace vms.entity.CustomDataAnnotations;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension))
            return new ValidationResult("Extenstion of the file is missing!");
        extension = extension.ToLower();
        if (!_extensions.Contains(extension))
            return new ValidationResult(GetErrorMessage(extension));
        else
            return ValidationResult.Success;
    }

    public string GetErrorMessage(string extensionName)
    {
        return $"This File extension {extensionName} is not allowed!";
    }
}