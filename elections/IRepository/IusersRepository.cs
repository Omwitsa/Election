using elections.IRepository;
using elections.Models;

namespace elections.Repository
{
	public interface IusersRepository : IGenericRepository<Users>
	{
		Users GetByUsername(string username);
	}
}
