using Microsoft.EntityFrameworkCore;

namespace elections.Models
{
	public class ElectionsDbContest : DbContext
	{
		public ElectionsDbContest() { }

		public ElectionsDbContest(DbContextOptions<ElectionsDbContest> options) : base(options)
		{

		}

		public virtual DbSet<Users> Users { get; set; }
		public virtual DbSet<Token> Token { get; set; }
	}
}
