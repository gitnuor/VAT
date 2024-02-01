using System;

namespace vms.utility;

public static class MathOperation
{
	public static decimal RoundDecimal(this decimal value, int precision)
	{
		return Math.Round(value, precision);
	}

	public static decimal RoundDecimalTwoDigit(this decimal value)
	{
		return Math.Round(value, 2);
	}

	public static decimal RoundDecimalThreeDigit(this decimal value)
	{
		return Math.Round(value, 3);
	}

	public static decimal RoundDecimalFourDigit(this decimal value)
	{
		return Math.Round(value, 4);
	}

	public static decimal RoundDecimalFiveDigit(this decimal value)
	{
		return Math.Round(value, 5);
	}

	public static decimal RoundDecimalSixDigit(this decimal value)
	{
		return Math.Round(value, 6);
	}

	public static decimal RoundDecimalSevenDigit(this decimal value)
	{
		return Math.Round(value, 7);
	}

	public static decimal RoundDecimalEightDigit(this decimal value)
	{
		return Math.Round(value, 8);
	}
}