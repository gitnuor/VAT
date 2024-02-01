using System;

namespace vms.utility;

public static class ExcelDataConverter
{
    public static bool GetBoolean(string data)
    {
        var result = false;
        if (string.IsNullOrEmpty(data)) return false;
        if (data.Trim() == "1" || data.Trim().ToLower() == "true")
        {
            result = true;
        }

        return result;
    }

    public static DateTime GetDatetime(string date)
    {
        return DateTime.TryParse(date, result: out var returnDate) ? returnDate : DateTime.Now;
    }

    public static DateTime? GetNullableDatetime(string date)
    {
        if (DateTime.TryParse(date, result: out var returnDate))
        {
            return returnDate;
        }
        return null;
    }

    public static int? GetNullableInt(string value="")
    {
        try
        {
            if (value == "")
            {
                return null;
            }
            return Convert.ToInt32(value);
        }
        catch
        {
            return null;
        }            
    }

    public static decimal? GetNullableDecimal(string value = "")
    {
        try
        {
            if (value == "")
            {
                return null;
            }

            return Convert.ToDecimal(value);
        }
        catch
        {
            return null;
        }
            
    }
}