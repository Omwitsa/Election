using elections.IRepository;
using elections.IServices;
using elections.Repository;
using elections.RequestModels;
using elections.Services;
using elections.UnitTests.Database;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace elections.UnitTests
{
	public class UserServicesTest
	{
		private IUnitOfWork electionUnitOfWork;
		private ElectionDbTestContext electionDbTest;
		private UnisolApiProxy unisolApiProxy;
		private IConfiguration _configuration;
		private IAuthService _authService;
		private UserServices userServices;
		private EmailService _emailService;
		public UserServicesTest()
		{
			// Arrange
			electionDbTest = new ElectionDbTestContext();
			electionUnitOfWork = new UnitOfWork(electionDbTest.GetContext());
			unisolApiProxy = new UnisolApiProxy("http://localhost:8088/api/");
			_emailService = new EmailService();
			_authService = new AuthService(_configuration, electionUnitOfWork);
			//var builder = new ConfigurationBuilder();
			//_configuration = builder.Build();
			_configuration = new Mock<IConfiguration>().Object;
			userServices = new UserServices(electionUnitOfWork, unisolApiProxy, _configuration, _emailService, _authService);
		}

		[Fact]
		public void RegisterUser_IfRegisterered_SuccessTrue()
		{
			var userData = new UserRegister { Username = "AGA311-0003/2018", Password = "123456", ConfirmPassword = "123456" };
			var registration = userServices.RegisterUser(userData, "Active");
			Assert.True(registration.Success);
		}

		[Fact]
		public void Login_IfFound_SuccessTrue()
		{
			var userData = new UserRegister { Username = "dean@abno.com", Password = "123456" };
			var registration = userServices.Login(userData);
			Assert.True(registration.Success);
		}
	}
}
