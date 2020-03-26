using elections.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elections.IServices
{
	public interface IUnisolApiProxy
	{
		Task<string> CheckStudentExist(string userCode, string classStatus);
	}
}
