using Microsoft.Extensions.Configuration;

namespace elections.Utilities
{
	public class DbSetting
	{ 
		public static string ConnectionString(IConfiguration configuration, string key)
		{
			var serverIp = configuration["DatabaseConnection:" + key + ":ServerIp"];
			var dbName = configuration["DatabaseConnection:" + key + ":DbName"];
			var dbUser = configuration["DatabaseConnection:" + key + ":DbUser"];
			var dbPassword = configuration["DatabaseConnection:" + key + ":DbPassword"];

			dbUser = Encrypter.Decrypt(dbUser);
			dbPassword = Encrypter.Decrypt(dbPassword);

			var conString = $"Server={serverIp};Database={dbName};User Id={dbUser};password={dbPassword};";
			return conString;
		}
	}
}
