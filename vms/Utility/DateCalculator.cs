using System;

namespace vms.Utility;

public static class DateCalculator
{
    public static DateTime GetLastDateOfMonth(int year, int month)
    {
        var firstDayOfMonth = new DateTime(year, month, 1);
        return firstDayOfMonth.AddMonths(1).AddDays(-1);
    }

    public static DateTime GetFirstDateOfMonth(int year, int month)
    {
        return new DateTime(year, month, 1);
    }
}