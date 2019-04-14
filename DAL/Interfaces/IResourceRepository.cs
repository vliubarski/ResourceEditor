using System.Collections.Generic;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IResourceRepository
	{
		IEnumerable<DbResource> GetResourceByString(string str);
		bool CreateResource(DbResource subStr);
		DbResource DeleteResource(DbResource subStr);
	}
}
