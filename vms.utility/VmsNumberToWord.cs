using System;

namespace vms.utility;

public static class VmsNumberToWord
{
    private static readonly string[] EngUnitNames =
    {
        "Zero", "One", "Two", "Three", "Four", "Five", "Six",
        "Seven", "Eight", "Nine", "Ten", "Eleven",
        "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen"
    };

    private static readonly string[] EngTenthNames =
    {
        "", "", "Twenty", "Thirty", "Forty",
        "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    };

    public static string ConvertAmountInEng(decimal amount)
    {
        try
        {
            var amountInt = (long) amount;
            var amountDec = (long) Math.Round((amount - amountInt) * 100);
            if (amountDec == 0)
            {
                return ConvertInEng(amountInt) + " Only.";
            }

            return ConvertInEng(amountInt) + " Point " + ConvertInEng(amountDec) + " Only.";
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string ConvertAmountUsingTakaPoishaInEng(decimal amount)
    {
        try
        {
            var amountInt = (long) amount;
            var amountDec = (long) Math.Round((amount - amountInt) * 100);
            if (amountDec == 0)
            {
                return "Taka: " + ConvertInEng(amountInt) + " Only.";
            }

            return "Taka: " + ConvertInEng(amountInt) + " And Paisa " + ConvertInEng(amountDec) + " Only.";
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private static string ConvertInEng(long i)
    {
        return i switch
        {
            < 20 => EngUnitNames[i],
            < 100 => EngTenthNames[i / 10] + ((i % 10 > 0) ? " " + ConvertInEng(i % 10) : ""),
            < 1000 => EngUnitNames[i / 100] + " Hundred " + ((i % 100 > 0) ? " " + ConvertInEng(i % 100) : ""),
            < 100000 => ConvertInEng(i / 1000) + " Thousand " + ((i % 1000 > 0) ? " " + ConvertInEng(i % 1000) : ""),
            < 10000000 => ConvertInEng(i / 100000) + (i / 100000 < 2 ? " Lakh " : " Lakhs ") + ((i % 100000 > 0) ? " " + ConvertInEng(i % 100000) : ""),
            _ => ConvertInEng(i / 10000000) + (i / 10000000 < 2?" Crore " : " Crores ")  + ((i % 10000000 > 0) ? " " + ConvertInEng(i % 10000000) : "")
        };
    }
}