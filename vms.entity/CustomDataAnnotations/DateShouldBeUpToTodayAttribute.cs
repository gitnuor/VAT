using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.CustomDataAnnotations;

public class DateShouldBeUpToTodayAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var dt = (DateTime) value;

        return dt <= DateTime.Now && dt >= new DateTime(2000, 1, 1);
    }
}