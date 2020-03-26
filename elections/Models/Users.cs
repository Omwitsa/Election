using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elections.Models
{
	public class Users
	{
		public Users()
		{
			Id = Guid.NewGuid();
		}
		public Guid Id { get; set; }
		public DateTime DateCreated { get; set; }  
		public string Email { get; set; }
		public bool EmailConfirmed { get; set; }
		public string PasswordHash { get; set; }
		public string PhoneNumber { get; set; }
		public string UserName { get; set; }
		public bool Status { get; set; }
		public Membership Membership { get; set; }
		public Level Level { get; set; }
	}
}

public enum Membership
{
	Ordinary = 1,
	Associate = 2
}

public enum Level
{
	Dean = 1
}