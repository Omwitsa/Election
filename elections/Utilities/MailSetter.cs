using elections.RequestModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace elections.Utilities
{
	public class MailSetter
	{
		public EmailMessage GetMailMessage(IConfiguration configuration, string email, string name, string userCode)
		{
			email = string.IsNullOrEmpty(configuration["DefaultValues:Email"]) ? email : configuration["DefaultValues:Email"];
			var toAddress = new EmailAddress
			{
				Name = name + "<" + userCode + ">",
				Address = email
			};

			var institutionEmail = configuration["EmailConfigs:EmailUserName"];
			var fromAddress = new EmailAddress
			{
				Name = configuration["DefaultValues:InstitutionName"],
				Address = institutionEmail
			};

			var subjectDefinition = "Account Creation";
			var UnisolPortalUrl = configuration["DefaultValues:ElectionPortalUrl"];
			var varificationLink = UnisolPortalUrl + "users/confirmAccount?userCode="+ userCode;
			var currentYear = configuration["DefaultValues:CurrentYear"];
			return new EmailMessage
			{
				ToAddresses = new List<EmailAddress> { toAddress },
				Content = GetMailMessage(name, varificationLink, fromAddress, currentYear),
				Subject = subjectDefinition,
				FromAddresses = new List<EmailAddress> { fromAddress },
			};
		}

		public EmailConfigs GetEmailConfigs(IConfiguration configuration)
		{
			return new EmailConfigs
			{
				SmtpClient = configuration["EmailConfigs:SmtpClient"],
				Port = configuration["EmailConfigs:Port"],
				EmailUserName = configuration["EmailConfigs:EmailUserName"],
				Password = configuration["EmailConfigs:Password"]
			};
		}
		
		private string GetMailMessage(string names, string varificationLink, EmailAddress fromAddress, string currentYear)
		{
			var message = "<div style='margin: 2em 5em 2em 5em; background-color: #f2f2f2'>" +
							"<table style='width: 100 %; margin: 5% 10% 5% 10%;'><br>" +
								//"<tr><td><img src='cid:logoId' style='width:200px; display: block; margin-left: auto; margin-right: auto;'/></td></tr>" +
								"<tr><td><h2 style='text-align: center; color: red'> Account Created Successfully <br></h2></td></tr>" +
								"<tr><td><h4>Dear " + names + ",</h4></td></tr>" +
								"<tr><td> Your Account has been created. Click on the 'Activate Account' button below to activate your account <br> <br></td></tr>" +
								"<tr><td style='text-align: center;'><a href='" + varificationLink + "' style='background-color: red; color: white; padding: 0.5em 1em; text-align: center; text-decoration: none; border-radius: 0.5em;'> Activate Account <br></a></td></tr> " +
								"<tr><td><p><span style='font-weight: bold'> Disclaimer:- </span> <i>The content of this email is confidential and intended for the recipient specified in this message only. " +
								"It is strictly forbidden to share any part of this message with any third party. " +
								"If you received this message by mistake, please reply to this message and follow with its deletion, " +
								"so that we can ensure such a mistake does not occur in the future.</i> </p></td></tr>" +
								"<tr><td><p>Sincerely, <br><br> <span style='font-weight: bold'> " + fromAddress.Name + " </span><br></p></td></tr> " +
							" </table>" +
							"<p style='text-align: center'>Powered By <a href='http://www.abnosoftwares.co.ke/' target='_blank' style='color:blue;'>" +
							"<b>ABNO Softwares International Ltd.</b></a> &copy; Copyright <span id='c-year'>" + currentYear + "</span></p><br>" +
						"</div>";
			return message;
		}
	}
}
