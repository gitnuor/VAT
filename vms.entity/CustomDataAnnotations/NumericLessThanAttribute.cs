using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using static System.String;

namespace vms.entity.CustomDataAnnotations;

public class NumericLessThanAttribute : ValidationAttribute
{
    private const string LessThanErrorMessage = "{0} must be less than {1}.";
    private const string LessThanOrEqualToErrorMessage = "{0} must be less than or equal to {1}.";

    public string OtherProperty { get; private set; }

    private bool _allowEquality;

    public bool AllowEquality
    {
        get => _allowEquality;
        set
        {
            _allowEquality = value;
            ErrorMessage = (value ? LessThanOrEqualToErrorMessage : LessThanErrorMessage);
        }
    }

    public NumericLessThanAttribute(string otherProperty)
        : base(LessThanErrorMessage)
    {
        OtherProperty = otherProperty ?? throw new ArgumentNullException("otherProperty");
    }

    public override string FormatErrorMessage(string name)
    {
        return Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherProperty);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

        if (otherPropertyInfo == null)
        {
            return new ValidationResult(Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty));
        }

        object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

        // Check to ensure the validating property is numeric
        if (!decimal.TryParse(value.ToString(), out var decValue))
        {
            return new ValidationResult(Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
        }

        // Check to ensure the other property is numeric
        if (!decimal.TryParse(otherPropertyValue.ToString(), out var decOtherPropertyValue))
        {
            return new ValidationResult(Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
        }

        // Check for equality
        if (AllowEquality && decValue == decOtherPropertyValue)
        {
            return null;
        }
        // Check to see if the value is greater than the other property value
        else if (decValue > decOtherPropertyValue)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return null;
    }

    public static string FormatPropertyForClientValidation(string property)
    {
        if (property == null)
        {
            throw new ArgumentException("Value cannot be null or empty.", "property");
        }
        return "*." + property;
    }
}