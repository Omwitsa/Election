using elections.Models;
using elections.Utilities;
using Microsoft.EntityFrameworkCore;
using System;

namespace elections.UnitTests.Database
{
	public class ElectionDbTestContext
	{
		public ElectionsDbContest GetContext()
		{
			var options = new DbContextOptionsBuilder<ElectionsDbContest>()
							  .UseInMemoryDatabase(Guid.NewGuid().ToString())
							  .Options;
			var context = new ElectionsDbContest(options);

			context.Users.Add(new Users
			{
				UserName = "dean@abno.com",
				DateCreated = DateTime.UtcNow,
				EmailConfirmed = true,
				PasswordHash = SecurePasswordHasher.Hash("123456"),
				PhoneNumber = "0715507260",
				Status = true,
				Level = Level.Dean,
			});

			context.SaveChanges();
			return context;
		}
	}
}
