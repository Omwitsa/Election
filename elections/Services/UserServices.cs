using elections.IRepository;
using elections.IServices;
using elections.Models;
using elections.Repository;
using elections.RequestModels;
using elections.ResponseModels;
using elections.Utilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;

namespace elections.Services
{
	public class UserServices : IUserServices
	{
		private IUnitOfWork _electionUnitOfWork;
		private IUnisolApiProxy _unisolApiProxy;
		private IConfiguration _configuration;
		private IEmailService _emailService;
		private IAuthService _authService;
		private InputValidator inputValidator = new InputValidator();
		private MailSetter mailSetter = new MailSetter();
		private Logger logger = LogManager.GetLogger("fileLogger");
		public UserServices(IUnitOfWork electionUnitOfWork, IUnisolApiProxy unisolApiProxy, IConfiguration configuration,  
			IEmailService emailService, IAuthService authService)
		{
			_electionUnitOfWork = electionUnitOfWork;
			_unisolApiProxy = unisolApiProxy;
			_configuration = configuration;
			_emailService = emailService;
			_authService = authService;
		}

		public ReturnData ConfirmAccount(string userCode)
		{
			try
			{
				var user = _electionUnitOfWork.Users.GetFirstOrDefault(u => u.UserName.ToLower().Equals(userCode.ToLower()));
				user.EmailConfirmed = true;
				_electionUnitOfWork.Save();

				return new ReturnData
				{
					Success = true,
					Message = "Email confirmed successfully"
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t AccountConfirmationError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occorred"
				};
			}
		}

		public ReturnData GetUsers()
		{
			try
			{
				var users = _electionUnitOfWork.Users.GetAll();
				return new ReturnData
				{
					Success = true,
					Data = users
				};
			}
			catch(Exception ex)
			{
				logger.Error($"\t GetUsersServiceError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occcurred"
				};
			}
		}

		public ReturnData Login(UserRegister loginDetails)
		{
			var requiredFields = new List<Tuple<string, string, DataType>>
				{
					Tuple.Create("username", loginDetails.Username, DataType.Default),
					Tuple.Create("password", loginDetails.Password, DataType.Password),
				};

			var validUserInputs = inputValidator.ValidateInputs(requiredFields);
			if (!validUserInputs.Success)
				return new ReturnData
				{
					Success = validUserInputs.Success,
					Message = "Sorry, either username or password is wrong. Kindly try again."
				};

			try
			{
				var user = _electionUnitOfWork.Users.GetFirstOrDefault(u => u.UserName.ToLower().Equals(loginDetails.Username.ToLower()));
				if (!SecurePasswordHasher.Verify(loginDetails.Password, user.PasswordHash))
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, either username or password is wrong. Kindly try again.",
					};

				//var tokenResponse = _authService.GetAccessToken();
				//if (!tokenResponse.Success)
				//	return tokenResponse;

				//var oldToken = _electionUnitOfWork.Token.GetByValue(t => t.UsersId.ToLower().Equals(user.Id.ToString().ToLower()));
				//if (oldToken != null)
				//	_electionUnitOfWork.Token.Remove(oldToken);

				//_electionUnitOfWork.Token.Add(new Token
				//{
				//	AccessToken = tokenResponse.Data.accessToken,
				//	RefreshToken = tokenResponse.Data.refreshToken,
				//	UsersId = user.Id.ToString(),
				//	DateCreated = DateTime.UtcNow,
				//	Revoked = false,
				//	AccessExpireMinutes = DateTimeOffset.Now.AddMinutes(5),
				//	RefreshExpireMinutes = DateTimeOffset.Now.AddMinutes(100)
				//});

				//_electionUnitOfWork.Save();

				return new ReturnData {
					Success = true
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t LoginServiceError: \t {ex}");
				return new ReturnData
				{
					Success = false
				};
			}
		}

		public ReturnData RegisterUser(UserRegister register, string classStatus)
		{
			try
			{
				var requiredFields = new List<Tuple<string, string, DataType>>
				{
					Tuple.Create("username", register.Username, DataType.Default),
					Tuple.Create("password", register.Password, DataType.Password),
					Tuple.Create("Confirm Password", register.ConfirmPassword, DataType.Password)
				};

				var validUserInputs = inputValidator.ValidateInputs(requiredFields);
				if (!validUserInputs.Success)
					return new ReturnData
					{
						Message = validUserInputs.Message,
						Success = validUserInputs.Success
					};

				if (!register.Password.Equals(register.ConfirmPassword))
					return new ReturnData
					{
						Success = false,
						Message = "Entered password do not match"
					};

				var member = _electionUnitOfWork.Users.GetFirstOrDefault(u => u.UserName.ToLower().Equals(register.Username.ToLower()));
				if (member != null)
					return new ReturnData
					{
						Success = false,
						Message = "Username already registered"
					};

				var studentDetails = _unisolApiProxy.CheckStudentExist(register.Username, classStatus).Result;
				var jData = JsonConvert.DeserializeObject<ReturnData>(studentDetails);
				if (!jData.Success)
					return new ReturnData
					{
						Success = jData.Success,
						Message = jData.Message
					};

				string email = jData.Data.email;


				_electionUnitOfWork.Users.Add(new Users
				{
					DateCreated = DateTime.UtcNow,
					UserName = register.Username,
					PasswordHash = SecurePasswordHasher.Hash(register.Password),
					Email = email,
					PhoneNumber = jData.Data.telno,
					Status = true,
				});

				_electionUnitOfWork.Save();

				string erpNames = jData.Data.names;
				var emailMessage = mailSetter.GetMailMessage(_configuration, email, erpNames, register.Username);
				var emailConfigs = mailSetter.GetEmailConfigs(_configuration);
				var mailDetails = _emailService.Send(emailMessage, emailConfigs);

				if (!mailDetails.Success)
					logger.Warn($"\t RegisterUserServiceWarning: \t {mailDetails.Message}");

				return new ReturnData {
					Success = true,
					Message = mailDetails.Message
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t RegisterUserServiceError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred"
				};
			}
		}

		public ReturnData ResetPasword(Users user)
		{
			try
			{
				//var searchData = db.PasswordHash.Where(x => x.GetH == resetPasword).SingleOrDefault();
				//if (searchData != null)
				//{
				//    return Json(new ReturnData
				//    {
				//        Success = true
				//    });
				//}
				//else
				//{
				//    //if (!SecurePasswordHasher.Verify("Username here", "Password here"))
				//    //    return Json(new ReturnData
				//    //    {
				//    //        Success = false,
				//    //        Message = "Sorry, you have entered a wrong password",
				//    //    });

				return new ReturnData
				{
					Success = true
				};
			}
			catch(Exception ex)
			{
				logger.Error($"\t ResetPaswordServiceError: \t {ex}");
				return new ReturnData
				{
					Success = false
				};
			}
		}
	}
}
