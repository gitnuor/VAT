using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace vms.entity.CustomDataAnnotations;

public class DateLessThanAttribute : ValidationAttribute
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

    public DateLessThanAttribute(string otherProperty)
        : base(LessThanErrorMessage)
    {
        OtherProperty = otherProperty ?? throw new ArgumentNullException("otherProperty");
    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherProperty);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

        if (otherPropertyInfo == null)
        {
            return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty));
        }

        object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

        // Check to ensure the validating property is numeric
        if (!DateTime.TryParse(value.ToString(), out var dateValue))
        {
            return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
        }

        // Check to ensure the other property is numeric
        if (!DateTime.TryParse(otherPropertyValue.ToString(), out var datOtherPropertyValue))
        {
            return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
        }

        // Check for equality
        if (AllowEquality && dateValue == datOtherPropertyValue)
        {
            return null;
        }
        // Check to see if the value is greater than the other property value
        else if (dateValue > datOtherPropertyValue)
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