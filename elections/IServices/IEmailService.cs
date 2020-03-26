using elections.RequestModels;
using elections.ResponseModels;


namespace elections.IServices
{
	public interface IEmailService
	{
		ReturnData Send(EmailMessage emailMessage, EmailConfigs emailSettings);
	}
}
