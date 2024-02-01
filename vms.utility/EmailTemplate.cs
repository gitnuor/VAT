namespace vms.utility;

public static class EmailTemplate
{
	public static string GetCreditLimitReminder(string customerName, decimal amount, string challanNo)
	{
		return @$"<div>	
						<p>
							Dear <span  style='font-weight: bold;'>{customerName}</span>,
						</p>
						<p>
							I hope this email finds you well. We value your relationship with us, and we hope that our services have been helpful to you. We understand that things can get busy, and we don’t want this to slip through the cracks.
						</p>
						<p>
							By taking care of your outstanding payment of {amount} for Challan No: {challanNo}, you will be able to continue to enjoy our services without interruption. Your prompt attention to this matter will help us keep our records up to date and ensure that you receive the best possible service.
						</p>
						<p>
							Thank you for your prompt attention to this matter.
						</p>
				</div>";
	}

	public static string GetInvoiceDue(string customerName, decimal amount, string challanNo)
	{
		return @$"<div>	
						<p>
							Dear <span  style='font-weight: bold;'>{customerName}</span>,
						</p>
						<p>
							I hope this email finds you well. I wanted to remind you that your Challan No: {challanNo} for {amount} is due today. If you could take a moment to take care of this, it would be greatly appreciated.
						</p>
						<p>
							Thank you for your prompt attention to this matter.
						</p>
				</div>";
	}

	public static string GetVdsCertificateDue(string customerName, decimal amount, string challanNo)
	{
		return @$"<div>	
						<p>
							Dear <span  style='font-weight: bold;'>{customerName}</span>,
						</p>
						<p>
							I hope this email finds you well. I wanted to remind you that Mushak 6.6 for {amount} with referring to Mushak 6.3 No {challanNo} is due today. If you could take a moment to take care of this, it would be greatly appreciated.
						</p>
						<p>
							Thank you for your prompt attention to this matter.
						</p>
				</div>";
	}

	public static string GetAitCertificateDue(string customerName, decimal amount, string challanNo)
	{
		return @$"<div>	
						<p>
							Dear <span  style='font-weight: bold;'>{customerName}</span>,
						</p>
						<p>
							I hope this email finds you well. I wanted to remind you that AIT certificate for {amount} with referring to Mushak 6.3 No {challanNo} is due today. If you could take a moment to take care of this, it would be greatly appreciated.
						</p>
						<p>
							Thank you for your prompt attention to this matter.
						</p>
				</div>";
	}
}