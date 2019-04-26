using System.Collections.Generic;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IResourceRepository
	{
		IEnumerable<DbResource> GetResourceByString(string str);
		bool CreateResource(DbResource subStr);
		bool  DeleteResource(DbResource subStr);
	}
}
