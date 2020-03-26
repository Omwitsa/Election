using elections.IRepository;
using elections.IServices;
using elections.Repository;
using elections.Services;
using elections.UnitTests.Database;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace elections.UnitTests
{
	public class AuthServiceTest
	{
		private IConfiguration _configuration;
		private IAuthService authService;
		private IUnitOfWork electionUnitOfWork;
		private ElectionDbTestContext electionDbTest;
		public AuthServiceTest()
		{
			electionDbTest = new ElectionDbTestContext();
			electionUnitOfWork = new UnitOfWork(electionDbTest.GetContext());
			_configuration = new Mock<IConfiguration>().Object;
			authService = new AuthService(_configuration, electionUnitOfWork);
		}

		[Fact]
		public void GetAccessToken_Found_ReturnSuccess()
		{
			var token = authService.GetAccessToken();
			Assert.True(token.Success);
		}

		[Fact]
		public void RefreshTokenTest() // response.StatusCode == HttpStatusCode.Unauthorized -- condition to request refresh token
		{
			//HttpClient _client = new HttpClient();

			//var rsUrl = "http://localhost:5002/api/users/GetUsers";
			//_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "Expired token");
			//HttpResponseMessage rsMsg = _client.GetAsync(rsUrl).Result;

			//if (rsMsg.StatusCode == HttpStatusCode.Unauthorized)
			//{
			//	// Refresh the token

			//	_client.DefaultRequestHeaders.Clear();
			//	_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "refreshed token");
			//	HttpResponseMessage rsMsgNew = _client.GetAsync(rsUrl).Result;
			//}
		}
	}
}
