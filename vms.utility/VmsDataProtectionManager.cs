using System;
using Microsoft.AspNetCore.DataProtection;

namespace vms.utility;

public static class VmsDataProtectionManager
{
	private static readonly IDataProtectionProvider VmsDataProtectionProvider =
		DataProtectionProvider.Create(("iVATDataProtectionProvider"));

	private static readonly IDataProtector VmsDataProtector =
		VmsDataProtectionProvider.CreateProtector("iVATDataProtection");

	public static string EncryptString(string plainText)
	{
		return VmsDataProtector.Protect(plainText);
	}

	public static string EncryptInt(int intToEncrypt)
	{
		return EncryptString(intToEncrypt.ToString());
	}

	public static string DecryptString(string cipherText)
	{
		return VmsDataProtector.Unprotect(cipherText);
	}

	public static int DecryptInt(string cipherText)
	{
		var clearText = DecryptString(cipherText);
		try
		{
			return int.Parse(clearText);
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
	}
}