using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace vms.utility;

public  class MailMaster
{
    public static void SendMail(string mailTo,string subject, string mailBody)
    {
	    var fromAddress = new MailAddress("finance.trucklagbe@outlook.com", "Finance-Accounts");
		// var fromAddress = new MailAddress("bitsvms@gmail.com", "Brac IT Services Limited");
        var toAddress = new MailAddress(mailTo, "");
        // const string fromPassword = "vms12345*";
        const string fromPassword = "F!n@nce&Account$";
		//const string subject = "Test Gmail";
		//const string body = "You are doing Great";

		// var smtp = new SmtpClient
  //       {
  //           Host = "smtp.gmail.com",
  //           Port = 587,
  //           EnableSsl = true,
  //           DeliveryMethod = SmtpDeliveryMethod.Network,
  //           UseDefaultCredentials = false,
  //           Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
  //       };

		var smtp = new SmtpClient
        {
            Host = "smtp.office365.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
        };

        using (var message = new MailMessage(fromAddress, toAddress)
               {
                   Subject = subject,
                   Body = mailBody

               })
        {
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }

    public static void SendEmailWithAttachment(string toAddress, string subject, string body, Dictionary<string, Stream> attachments = null, bool isHtml = true)
	{
		var fromAddress = new MailAddress("finance.trucklagbe@outlook.com", "Finance-Accounts");
		const string fromPassword = "F!n@nce&Account$";

		// Set up the email message
		var message = new MailMessage();
	    message.To.Add(toAddress);
	    message.Subject = subject;
	    message.Body = body;
		message.IsBodyHtml = isHtml;
	    message.From = fromAddress; // new MailAddress("sender@email.com");

	    // Add attachments, if any
	    if (attachments != null)
	    {
		    foreach (var fileAttachment in attachments.Select(attachment => new Attachment(attachment.Value, attachment.Key)))
		    {
			    message.Attachments.Add(fileAttachment);
		    }
	    }

	    // Set up the SMTP client and send the email
	    var smtpClient = new SmtpClient("smtp.office365.com")
	    {
		    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
		    DeliveryFormat = SmtpDeliveryFormat.SevenBit,
		    DeliveryMethod = SmtpDeliveryMethod.Network,
		    EnableSsl = true,
		    PickupDirectoryLocation = null,
		    Port = 587,
		    TargetName = null,
		    Timeout = 100000,
		    UseDefaultCredentials = false
	    };
	    smtpClient.Send(message);
	    if (attachments == null) return;
	    foreach (var fileAttachment in attachments.Select(
		             attachment => new Attachment(attachment.Value, attachment.Key)))
	    {
		    fileAttachment.Dispose();
	    }
	}
}