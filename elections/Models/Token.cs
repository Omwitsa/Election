using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elections.Models
{
	public class Token
	{
		public int Id { get; set; }
		public string UsersId { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime DateCreated { get; set; }
		public bool Revoked { get; set; }
		public DateTimeOffset AccessExpireMinutes { get; set; }
		public DateTimeOffset RefreshExpireMinutes { get; set; }
	}
}
