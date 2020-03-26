using elections.Services;
using elections.Utilities;
using System;
using System.Linq;

namespace elections.Models
{
	public static class ElectionDbContestExtention
	{
		public static void EnsureDatabaseSeeded(this ElectionsDbContest context)
		{
			if (!context.Users.Any())
			{ 
				context.Add(new Users
				{
					UserName = "dean@abno.com",
					DateCreated = DateTime.UtcNow,
					EmailConfirmed = true,
					PasswordHash = SecurePasswordHasher.Hash("123456"),
					PhoneNumber = "0715507260",
					Status = true,
					Level = Level.Dean,

				});
			}

			context.SaveChanges();
		}
	}
}
