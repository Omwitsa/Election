using elections.Models;
using elections.RequestModels;
using elections.ResponseModels;

namespace elections.IServices
{
	public interface IUserServices
	{
		ReturnData RegisterUser(UserRegister register, string classStatus);
		ReturnData GetUsers();
		ReturnData Login(UserRegister user);
		ReturnData ResetPasword(Users user);
		ReturnData ConfirmAccount(string userCode);
	}
}
