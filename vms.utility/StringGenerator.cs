using System;
using System.Globalization;

namespace vms.utility;

public static class StringGenerator
{
	#region Decimal

	public static string DecimalToCommaSeparatedByDigit(this decimal? input, int numberAfterDecimal = 2,
		bool isEmptyForNull = false)
	{
		if (input == null && isEmptyForNull)
			return string.Empty;
		input ??= 0;
		input = Math.Round(input.Value, numberAfterDecimal);
		if (numberAfterDecimal < 1)
		{
			return input.Value.ToString("#,0;(#,0)", new CultureInfo("en-IN", false));
		}

		var afterDecimalString = new string('0', numberAfterDecimal);
		return input.Value.ToString($"#,0.{afterDecimalString};(#,0.{afterDecimalString})",
			new CultureInfo("en-IN", false));
	}

	public static string DecimalToCommaSeparatedWithZeroDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(0, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithOneDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(1, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithTwoDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(2, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithThreeDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(3, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithFourDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(4, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithFiveDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(5, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithSixDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(6, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithSevenDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(7, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithEightDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(8, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithNineDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(9, isEmptyForNull);
	}

	public static string DecimalToCommaSeparatedWithTenDigit(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedByDigit(10, isEmptyForNull);
	}

	public static string DecimalToCommaSeparated(this decimal? input, bool isEmptyForNull = false)
	{
		return input.DecimalToCommaSeparatedWithTwoDigit(isEmptyForNull);
	}

	#endregion

	#region DateTime

	public static string DateTimeToString(DateTime? dateTime, bool isIncludeTime = true)
	{
		return dateTime == null
			? string.Empty
			: dateTime.Value.ToString(isIncludeTime ? "dd/MM/yyyy hh:mm:ss tt" : "dd/MM/yyyy");
	}

	public static string DateTimeToTimeString(DateTime? dateTime)
	{
		return dateTime == null ? string.Empty : dateTime.Value.ToString("hh:mm:ss tt");
	}

	public static string DateTimeToStringWithoutTime(this DateTime? dateTime)
	{
		return DateTimeToString(dateTime, false);
	}

	public static object DateTimeToSqlCompatibleStringWithoutDbNull(DateTime? dateToConvert)
	{
		return dateToConvert?.ToString("yyyy-MM-dd HH:mm:ss.fff");
	}

	public static object DateTimeToSqlCompatibleString(DateTime? dateToConvert)
	{
		if (dateToConvert == null)
			return DBNull.Value;
		return dateToConvert.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
	}

	#endregion

	#region Misc

	public static string EmptyStringToNa(this string inputString)
	{
		return string.IsNullOrEmpty(inputString) ? "N/A" : inputString;
	}

	public static string BooleanToString(bool? val)
	{
		return val is null or false ? "No" : "Yes";
	}

	public static string RemoveWwwRoot(string mainString)
	{
		return mainString.Replace("/wwwroot/", @"\").Replace(@"\wwwroot\", @"\").Replace(@"/wwwroot\", @"\")
			.Replace(@"\wwwroot/", @"\").Replace("wwwroot/", @"\").Replace(@"wwwroot\", @"\");
	}

	public static string AddCurrentTimeToString(string inputStr)
	{
		return $"{inputStr}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
	}

	public static string CssClassForPaymentDue(DateTime salesDate, int? creditPeriod)
	{
		if (creditPeriod == null) return "";

		var dueDate = (DateTime.Now - salesDate).TotalDays;
		return dueDate > creditPeriod ? "bg-warning" : "";
	}

	#endregion
}