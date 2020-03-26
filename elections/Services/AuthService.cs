using elections.IRepository;
using elections.IServices;
using elections.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace elections.Services
{
	public class AuthService : IAuthService
	{
		private IConfiguration _configuration;
		private IUnitOfWork _electionUnitOfWork;
		public AuthService(IConfiguration configuration, IUnitOfWork electionUnitOfWork)
		{
			_configuration = configuration;
			_electionUnitOfWork = electionUnitOfWork;
		}

		public ReturnData GetAccessToken()
		{
			var issuer = _configuration["AuthSettings:Issuer"];
			var audience = _configuration["AuthSettings:Audience"];
			var sharedKey = _configuration["AuthSettings:SigningKey"];

			var userData = new
			{
				Username = sharedKey,
				Time = DateTime.UtcNow
			};
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, sharedKey),
				new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userData))
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sharedKey));
			var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
			var JwtToken = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims,
				signingCredentials: signInCredentials, expires: DateTime.UtcNow.AddMinutes(1));

			var accessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
			var refreshToken = GenerateRefreshToken();  //  Guid.NewGuid().ToString
			
			return new ReturnData
			{
				Success = true,
				Data = new
				{
					accessToken,
					refreshToken
				}
			};
		}
		// response.StatusCode == HttpStatusCode.Unauthorized -- condition to request refresh token
		public ReturnData GetRefreshToken(string refreshToken)
		{
			var token = _electionUnitOfWork.Token.GetFirstOrDefault(t => t.RefreshToken.ToLower().Equals(refreshToken.ToString()));
			if (token == null)
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, Invalid refresh token. Kindly, login again"
				};
			var accessToken = GetAccessToken();
			return accessToken;
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}
	}
}
