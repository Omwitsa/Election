using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elections.ResponseModels
{
	public class ReturnData
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public dynamic Data { get; set; }
	}
}
