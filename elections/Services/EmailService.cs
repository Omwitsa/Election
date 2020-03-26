using elections.IServices;
using elections.RequestModels;
using elections.ResponseModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Linq;

namespace elections.Services
{
	public class EmailService : IEmailService
	{
		public ReturnData Send(EmailMessage emailMessage, EmailConfigs emailConfigs)
		{
			try
			{
				var message = new MimeMessage();
				message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
				message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
				message.Subject = emailMessage.Subject;

				var builder = new BodyBuilder();
				//var image = builder.LinkedResources.Add(emailMessage.InstitutionLogo);
				//image.ContentId = "logoId";
				builder.HtmlBody = emailMessage.Content;
				message.Body = builder.ToMessageBody();

				var smtpPort = Int32.Parse(emailConfigs.Port);
				var options = smtpPort == 587 ? SecureSocketOptions.None : SecureSocketOptions.SslOnConnect;
				using (var emailClient = new SmtpClient())
				{
					emailClient.Connect(emailConfigs.SmtpClient, smtpPort, options);
					emailClient.AuthenticationMechanisms.Remove("XOAUTH2"); //Remove any OAuth functionality as we won't be using it.
					emailClient.Authenticate(emailConfigs.EmailUserName, emailConfigs.Password);

					emailClient.Send(message);
					emailClient.Disconnect(true);
				}

				return new ReturnData
				{
					Success = true,
					Message = "Kindly check your email to activate your account"
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "An error occured while sending an email, Kindly reset your password later"
				};
			}
		}

	}
}
