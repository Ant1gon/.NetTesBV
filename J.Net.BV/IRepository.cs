using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace J.Net.BV.Controllers
{
	interface IRepository<T>: IDisposable
		where T : class
	{
		public IEnumerable<T> Get();

		//public T Get(Guid id);
		public T Get(int id);

		//public int Post();
		public void Post();

		public void Delete(int id);
	}
}
