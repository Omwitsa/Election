using elections.IServices;
using elections.ResponseModels;
using elections.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;

namespace elections.Controllers
{
	[Route("api/[controller]")]
	public class AuthController : Controller
    {
		private IConfiguration _configuration;
		private IAuthService _authService;
		private Logger logger = LogManager.GetLogger("fileLogger");

		public AuthController(IConfiguration configuration, IAuthService authService)
		{
			_configuration = configuration;
			_authService = authService;
		}

		[HttpGet("encrypt")]
		public JsonResult Encrypt(string value)
		{
			var encryptValue = Encrypter.Encrypt(value);
			return Json(new ReturnData
			{
				Success = true,
				Data = encryptValue
			});
		}

		[HttpGet("[action]")]
		public JsonResult GetAccessToken()
		{
			var token = _authService.GetAccessToken();
			return Json(token);
		}

		[HttpGet("[action]")]
		public JsonResult GetRefreshToken(string refreshToken)
		{ // response.StatusCode == HttpStatusCode.Unauthorized -- condition to request refresh token
			var token = _authService.GetRefreshToken(refreshToken);
			return Json(token);
		}
	}
}