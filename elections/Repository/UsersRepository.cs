using elections.Models;
using System.Linq;

namespace elections.Repository
{
	public class UsersRepository : GenericRepository<Users>, IusersRepository
	{
		private ElectionsDbContest _context;
		public UsersRepository(ElectionsDbContest context) : base(context)
		{
			_context = context;
		}

		public Users GetByUsername(string username)
		{
			return _context.Users.FirstOrDefault(u => u.UserName.ToLower().Equals(username.ToLower()));
		}
	}
}
