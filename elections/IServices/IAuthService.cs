using elections.ResponseModels;
using Microsoft.Extensions.Configuration;

namespace elections.IServices
{
	public interface IAuthService
	{
		ReturnData GetAccessToken();
		ReturnData GetRefreshToken(string refreshToken);
	}
}
