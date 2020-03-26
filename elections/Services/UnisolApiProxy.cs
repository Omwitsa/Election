using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using RestSharp;
using elections.IServices;

namespace elections.Services
{
	public class UnisolApiProxy : IUnisolApiProxy
	{
		private readonly string _unisolApiUrl;
		private IConfiguration _configuration { get; }

		public UnisolApiProxy(IConfiguration _configuration)
		{
			_unisolApiUrl = _configuration["DefaultValues:UnisolApiUrl"];
		}

		public UnisolApiProxy(string unisolApiUrl)
		{
			_unisolApiUrl = unisolApiUrl;
		}

		private async Task<string> Get(string resourceUrl)
		{
			var restClient = new RestClient(_unisolApiUrl);
			var restRequest = new RestRequest(resourceUrl, Method.GET) { RequestFormat = DataFormat.Json };
			var data = await restClient.ExecuteGetTaskAsync(restRequest);
			return data.Content;
		}

		private async Task<string> Post(string resourceUrl, object entity)
		{
			var restClient = new RestClient(_unisolApiUrl);
			var restRequest = new RestRequest(resourceUrl, Method.POST) { RequestFormat = DataFormat.Json };
			restRequest.AddBody(entity);
			var response = await restClient.ExecutePostTaskAsync(restRequest);
			return response.Content;
		}

		public Task<string> CheckStudentExist(string userCode, string classStatus)
		{
			var data = new
			{
				RegNumber = userCode
			};

			var response = Post("users/CheckStudentExists/?classStatus=" + classStatus, data);
			return response;
		}
	}
}
