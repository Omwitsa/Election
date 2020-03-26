using elections.IServices;
using elections.Models;
using elections.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace elections.Controllers
{
	//[Authorize]
	[Route("api/[controller]")]
    public class UsersController : Controller
    {
		private string classStatus = "Active";
		private IUserServices _userServices;
		public UsersController(IUserServices userServices)
		{
			_userServices = userServices;
		}

		[Authorize]
		[HttpGet("[action]")]
        public JsonResult GetUsers()
        {
			var users = _userServices.GetUsers();
			return Json(users);
        }

		[HttpPost("[action]")]
		public JsonResult RegisterUser([FromBody] UserRegister register)
		{
			var userRegistration = _userServices.RegisterUser(register, classStatus);
			return Json(userRegistration);
		}

		[HttpPost("[action]")]
		public JsonResult Login([FromBody] UserRegister user)
		{
			var login = _userServices.Login(user);
			return Json(login);
		}

		[HttpPost("[action]")]
		public JsonResult ResetPasword([FromBody] Users user)
        {
			var reset = _userServices.ResetPasword(user);
			return Json(reset);
        }

		[HttpGet("[action]")]
		public JsonResult ConfirmAccount(string userCode)
		{
			var confirm = _userServices.ConfirmAccount(userCode);
			return Json(confirm);
		}
	}
}