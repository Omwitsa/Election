using elections.Models;
using elections.Repository;

namespace elections.IRepository
{
	public interface IUnitOfWork
	{
		// Customized repositories
		IusersRepository Users { get; }
		// General repositories
		IGenericRepository<Token> Token { get; }
		int Save();
	}
}
