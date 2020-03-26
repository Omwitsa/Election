using elections.IRepository;
using elections.Models;

namespace elections.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ElectionsDbContest _context;
		public UnitOfWork(ElectionsDbContest context)
		{
			_context = context;
			Users = new UsersRepository(_context);
			Token = new GenericRepository<Token>(_context);
		}

		public IusersRepository Users { get; }
		public IGenericRepository<Token> Token { get; }

		public int Save()
		{
			return _context.SaveChanges();
		}
	}
}
